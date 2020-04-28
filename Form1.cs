using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpTester
{
    public partial class Form1 : Form
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        public static readonly string SPLIT_REGEX = "([ \\t{}():;])";
        public static readonly string[] KW =
        {
            "public", "static", "readonly", "using", "private", "protected", "internal", "partial", "class", "var", "string", "int", "long", "void",
            "object", "namespace", "char", "is", "sizeof", "null", "ref", "out", "as", "new"
        };
        public static readonly string[] LOGIC_KW =
        {
            "for", "foreach", "in", "if", "continue", "break", "else"
        };
        public static string[] OBJECT_KW, ENUM_KW, METHOD_KW;
        public Form1()
        {
            InitializeComponent();

            IntPtr consoleHandle = GetConsoleWindow();
            ShowWindow(consoleHandle, SW_HIDE);

            input.Text = 
@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyNamespace
{
    static class Program
    {
        public static void Main(string[] args)
        {
            
        }
    }
}";
            InitObejctKW();
            OnNeedsSyntax(null, null);
        }
        void InitObejctKW()
        {
            List<string> refTypes = new List<string>();
            List<string> refTypesE = new List<string>();
            List<string> refMethods = new List<string>();
            foreach (Assembly ass in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type t in ass.GetExportedTypes())
                {
                    if (!t.IsEnum)
                        refTypes.Add(t.Name);
                    else
                        refTypesE.Add(t.Name);

                    foreach(MethodInfo inf in t.GetMethods())
                    {
                        refMethods.Add(Regex.Escape(inf.Name));
                    }
                }
            }
            OBJECT_KW = refTypes.ToArray();
            ENUM_KW = refTypesE.ToArray();
            METHOD_KW = refMethods.Distinct().ToArray();
        }
        int CountOpenersBefore(int point)
        {
            int count = 0;
            int chars = -1;
            foreach(char c in input.Text)
            {
                chars++;
                if (chars >= point)
                    break;
                if (c == '{')
                    count++;
                if (c == '}')
                    count--;
            }
            return count;
        }
        private static void DisplayErrors(IEnumerable<Diagnostic> errs)
        {
            foreach (Diagnostic diagnostic in errs)
            {
                MessageBox.Show("Line " + diagnostic.Location.SourceSpan.ToString() + ": " + diagnostic.GetMessage()
                    , "Compile-time error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return;
        }


        // events
        private void RunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SyntaxTree systree = CSharpSyntaxTree.ParseText(input.Text);

            Assembly[] assemblies = new List<Assembly> {
                typeof(object).GetTypeInfo().Assembly,
                typeof(Console).GetTypeInfo().Assembly,
                typeof(Enumerable).GetTypeInfo().Assembly,
                typeof(File).GetTypeInfo().Assembly,
                typeof(Assembly).GetTypeInfo().Assembly,
                typeof(CSharpArgumentInfo).GetTypeInfo().Assembly,
                typeof(CSharpSyntaxTree).GetTypeInfo().Assembly,
                typeof(System.Dynamic.CallInfo).GetTypeInfo().Assembly,
                typeof(RuntimeBinderException).GetTypeInfo().Assembly,
                typeof(System.Dynamic.ConvertBinder).GetTypeInfo().Assembly,
                typeof(ExpressionType).GetTypeInfo().Assembly,
                typeof(MessageBox).GetTypeInfo().Assembly,
                //Assembly.Load(new AssemblyName("Microsoft.CSharp")),
                //Assembly.Load(new AssemblyName("netstandard")),
                //Assembly.Load(new AssemblyName("System.Dynamic.Runtime")),
                //Assembly.Load(new AssemblyName("System.Runtime")),
                //Assembly.Load(new AssemblyName("mscorlib"))
            }.Distinct().ToArray();

            string[] assemblyPaths = new string[assemblies.Length];
            for (int x = 0; x < assemblies.Length; x++)
            {
                assemblyPaths[x] = assemblies[x].Location;
            }

            List<PortableExecutableReference> references = assemblyPaths.Select
                (r => MetadataReference.CreateFromFile(r)).ToList();

            var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            CSharpCompilation compilation = CSharpCompilation.Create(
                "csharptestbuild",
                syntaxTrees: new[] { systree },
                references: references,
                options: options);

            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);
                if (!result.Success)
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);
                    DisplayErrors(failures);
                    return;
                }
                else
                {
                    ms.Flush();
                    ms.Seek(0, SeekOrigin.Begin);
                    Assembly assembly = Assembly.Load(ms.ToArray());
                    var type = assembly.GetTypes()[0];
                    if(!type.GetMethods().Any(mi => mi.IsStatic))
                    {
                        MessageBox.Show("No (PUBLIC STATIC) method found to use as the entry point.");
                        return;
                    }
                    var methd = type.GetMethods().First(mi => mi.IsStatic) as MethodInfo;

                    try
                    {
                        bool alloc = allocConsoleToolStripMenuItem.Checked;
                        IntPtr consoleHandle = IntPtr.Zero;
                        if (alloc)
                        {
                            consoleHandle = GetConsoleWindow();
                            Console.Clear();
                            ShowWindow(consoleHandle, SW_SHOW);
                        }
                        methd.Invoke(null, new object[] { new string[] { "Hello", "World!" } });
                        if(consoleHandle != IntPtr.Zero)
                            ShowWindow(consoleHandle, SW_HIDE);
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message, "Runtime Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }
        private void input_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int selStart = input.SelectionStart;
                string txt = input.Text;
                if (txt[selStart - 1].Equals('{'))
                {

                }
                int count = CountOpenersBefore(selStart+1);
                if (count <= 0) return;
                StringBuilder sb = new StringBuilder();
                sb.Append(' ', count * 4);
                input.Text = txt.Insert(selStart, sb.ToString());
                input.SelectionStart = selStart+count*4;
            }
        }
        private void input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.OemCloseBrackets && e.Modifiers == Keys.Shift)
            {
                string txt = input.Text;
                int selStart = input.SelectionStart;
                if(!string.IsNullOrWhiteSpace(txt.Substring(selStart-4, 3)))
                    return;
                string remove4 = txt.Remove(selStart - 4, 4);
                input.Text = remove4;
                input.SelectionStart = selStart-4;
            }
        }
        string keywords = null, logic = null, objs = null, enums = null, methods = null, comments = null, strings = null;

        private void toEXEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BuildTo(BuildType.EXE);
        }
        private void toDLLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BuildTo(BuildType.DLL);
        }

        private void OnNeedsSyntax(object sender, EventArgs e)
        {
            string txt = input.Text;

            if(keywords == null)
                keywords = "\\b(" + string.Join("|", KW) + ")\\b";
            MatchCollection keywordMatches = Regex.Matches(txt, keywords);

            if (logic == null)
                logic = "\\b(" + string.Join("|", LOGIC_KW) + ")\\b";
            MatchCollection logicMatches = Regex.Matches(txt, logic);

            if (objs == null)
                objs = "\\b(" + string.Join("|", OBJECT_KW) + ")\\b";
            MatchCollection objMatches = Regex.Matches(txt, objs);

            if(enums == null)
                enums = "\\b(" + string.Join("|", ENUM_KW) + ")\\b";
            MatchCollection enumMatches = Regex.Matches(txt, enums);

            if(methods == null)
                methods = "\\b(" + string.Join("|", METHOD_KW) + ")\\b";
            MatchCollection methodMatches = Regex.Matches(txt, methods);

            if(comments == null)
                comments = @"(\/\/.+?$|\/\*.+?\*\/)";
            MatchCollection commentMatches = Regex.Matches(txt, comments, RegexOptions.Multiline);

            if(strings == null)
                strings = "\".+?\"";
            MatchCollection stringMatches = Regex.Matches(txt, strings);

            int originalIndex = input.SelectionStart;
            int originalLength = input.SelectionLength;
            Color originalColor = input.ForeColor;

            menuStrip1.Focus();

            input.SelectionStart = 0;
            input.SelectionLength = input.Text.Length;
            input.SelectionColor = originalColor;

            foreach (Match m in keywordMatches)
            {
                input.SelectionStart = m.Index;
                input.SelectionLength = m.Length;
                input.SelectionColor = Color.DodgerBlue;
            }

            foreach (Match m in logicMatches)
            {
                input.SelectionStart = m.Index;
                input.SelectionLength = m.Length;
                input.SelectionColor = Color.Plum;
            }

            foreach (Match m in objMatches)
            {
                input.SelectionStart = m.Index;
                input.SelectionLength = m.Length;
                input.SelectionColor = Color.Turquoise;
            }

            foreach (Match m in enumMatches)
            {
                input.SelectionStart = m.Index;
                input.SelectionLength = m.Length;
                input.SelectionColor = Color.Olive;
            }

            foreach (Match m in methodMatches)
            {
                input.SelectionStart = m.Index;
                input.SelectionLength = m.Length;
                input.SelectionColor = Color.PaleGoldenrod;
            }

            foreach (Match m in commentMatches)
            {
                input.SelectionStart = m.Index;
                input.SelectionLength = m.Length;
                input.SelectionColor = Color.ForestGreen;
            }

            foreach (Match m in stringMatches)
            {
                input.SelectionStart = m.Index;
                input.SelectionLength = m.Length;
                input.SelectionColor = Color.LightSalmon;
            }//"MMMMMMMMMMMMMMMMM"

            input.SelectionStart = originalIndex;
            input.SelectionLength = originalLength;
            input.SelectionColor = originalColor;

            input.Focus();
        }

        public enum BuildType { EXE, DLL }
        public void BuildTo(BuildType type)
        {
            SyntaxTree systree = CSharpSyntaxTree.ParseText(input.Text);

            Assembly[] assemblies = new List<Assembly> {
                typeof(object).GetTypeInfo().Assembly,
                typeof(Console).GetTypeInfo().Assembly,
                typeof(Enumerable).GetTypeInfo().Assembly,
                typeof(File).GetTypeInfo().Assembly,
                typeof(Assembly).GetTypeInfo().Assembly,
                typeof(CSharpArgumentInfo).GetTypeInfo().Assembly,
                typeof(CSharpSyntaxTree).GetTypeInfo().Assembly,
                typeof(System.Dynamic.CallInfo).GetTypeInfo().Assembly,
                typeof(RuntimeBinderException).GetTypeInfo().Assembly,
                typeof(System.Dynamic.ConvertBinder).GetTypeInfo().Assembly,
                typeof(ExpressionType).GetTypeInfo().Assembly,
                typeof(MessageBox).GetTypeInfo().Assembly,
                //Assembly.Load(new AssemblyName("Microsoft.CSharp")),
                //Assembly.Load(new AssemblyName("netstandard")),
                //Assembly.Load(new AssemblyName("System.Dynamic.Runtime")),
                //Assembly.Load(new AssemblyName("System.Runtime")),
                //Assembly.Load(new AssemblyName("mscorlib"))
            }.Distinct().ToArray();

            string[] assemblyPaths = new string[assemblies.Length];
            for (int x = 0; x < assemblies.Length; x++)
            {
                assemblyPaths[x] = assemblies[x].Location;
            }

            List<PortableExecutableReference> references = assemblyPaths.Select
                (r => MetadataReference.CreateFromFile(r)).ToList();

            CSharpCompilationOptions options;
            if (type == BuildType.DLL)
                options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            else
                options = new CSharpCompilationOptions(OutputKind.ConsoleApplication);
            CSharpCompilation compilation = CSharpCompilation.Create(
                "csharptestbuild",
                syntaxTrees: new[] { systree },
                references: references,
                options: options);

            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);
                if (!result.Success)
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);
                    DisplayErrors(failures);
                    return;
                }
                else
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    if (type == BuildType.DLL)
                        sfd.Filter = "Assembly (*.dll)|*.dll";
                    if (type == BuildType.EXE)
                        sfd.Filter = "Executable (*.exe)|*.exe";
                    if(sfd.ShowDialog() != DialogResult.OK)
                        return;
                    File.WriteAllBytes(sfd.FileName, ms.ToArray());
                }
            }
        }
    }
}
