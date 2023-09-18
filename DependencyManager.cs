using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace Multimedia
{
    public class DependencyManager
    {
        
        private static string pythonDir = GetPythonDir();

        static public (IntPtr ptr, dynamic sys) InitializePythonEnvironment()
        {
            string pythonVer = new DirectoryInfo(pythonDir).Name;
            string pythonDLL = Path.Combine(pythonDir, "p" + pythonVer.Trim('P') + ".dll");
            Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", pythonDLL);
            PythonEngine.Initialize();
            Console.WriteLine("Initialized Python engine");

            dynamic sys;
            using (Py.GIL())
            {
                sys = Py.Import("sys");
                dynamic io = Py.Import("io");
                dynamic stdout = io.StringIO();
                dynamic stderr = io.StringIO();
                sys.path.append(AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "/") + "Resources");
                sys.stdout = stdout;
                sys.stderr = stderr;
                sys.stdout.flush();
                sys.stderr.flush();
                Console.WriteLine("Initialized standard output and error");
            }
            return (ptr: PythonEngine.BeginAllowThreads(), sys);
        }
        static private string GetPythonDir()
        {
            DirectoryInfo pythonRootDir = new(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Programs\\Python"));
            DirectoryInfo[] pythonVersions = pythonRootDir.GetDirectories("Python" + "*.*");
            try
            {
                DirectoryInfo pythonKernel = pythonVersions.Last();
                pythonDir = pythonKernel.FullName;
            }
            catch
            {
                while(true)
                {
                    MessageBoxResult action = MessageBox.Show(
                        "This program requires Python3 to be installed.\nPress OK to locate your Python3 installation or Cancel to abort.", "Python kernel not found",
                        MessageBoxButton.OKCancel, MessageBoxImage.Warning);

                    if (action != MessageBoxResult.OK) Application.Current.Shutdown();

                    using FolderBrowserDialog dialog = new();
                    dialog.UseDescriptionForTitle = true;
                    dialog.Description = "Choose your Python3 installation directory";
                    dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    DialogResult choice = dialog.ShowDialog();
                    if (choice == DialogResult.Cancel) Application.Current.Shutdown();
                    string pythonExecutable = Path.Combine(dialog.SelectedPath, "python.exe");
                    if (File.Exists(pythonExecutable))
                    {
                        pythonDir = dialog.SelectedPath;
                        break;
                    }
                }
            }
            return pythonDir;
        }

        static public void CheckVidStabDependency()
        {
            while (true)
            {
                try
                {
                    using (Py.GIL())
                    {
                        Console.WriteLine("Checking if vidstab is installed...");
                        Py.Import("vidstab");
                        Console.WriteLine("Success!");
                    }
                    return;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    MessageBoxResult action = MessageBox.Show(
                        "This program requires the \"vidstab\" Python library in order to work.\nPress OK to install it through pip or Cancel to abort.", "Library not found",
                        MessageBoxButton.OKCancel, MessageBoxImage.Warning);

                    if (action != MessageBoxResult.OK)
                    {
                        Application.Current.Shutdown();
                        return;
                    }
                    InstallVidStab(pythonDir);
                }
            }
        }
        static private void InstallVidStab(string python)
        {
            string result = string.Empty;
            try
            {

                var info = new ProcessStartInfo(python)
                {
                    Arguments = "-m pip install vidstab[cv2]",
                    RedirectStandardInput = false,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                };

                using (var proc = new Process())
                {
                    proc.StartInfo = info;
                    proc.Start();
                    proc.WaitForExit();
                    if (proc.ExitCode == 0)
                    {
                        result = proc.StandardOutput.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to install vidstab dependency: " + result, ex);
            }
        }
    }
}
