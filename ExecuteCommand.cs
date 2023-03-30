using System;
using System.Threading;
using System.Diagnostics;

namespace PhotoToCSV
{
	public class ExecuteCommand
	{
        // StdOut result of command
		public string StdOut { get; set; }  = "";
        // StdErr result of command
        public string StdErr { get; set; } = "";
        // Exit code of command
        public int ExitCode { get; set; } = 0;

        // Execute a command by calling the command processor on the string cmd.
        // Returns stdOut concatenated with stdErr.
		public string ExecuteDOSCommandSync(string cmd)
		{
            string program = Environment.ExpandEnvironmentVariables("\"%COMSPEC%\"");
			string args = "/c " + cmd;
            ProcessStartInfo psi = new ProcessStartInfo(program, args);
			psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
			psi.RedirectStandardOutput = true;
			psi.RedirectStandardError = true;
			Thread thread_ReadStandardError = new Thread(new ThreadStart(Thread_ReadStandardError));
			Thread thread_ReadStandardOut = new Thread(new ThreadStart(Thread_ReadStandardOut));

			activeProcess = Process.Start(psi);
            if (activeProcess == null) 
            {
                return "";
            }
			if (psi.RedirectStandardError)
			{
				thread_ReadStandardError.Start();
			}
			if (psi.RedirectStandardOutput)
			{
				thread_ReadStandardOut.Start();
			}
			activeProcess.WaitForExit();
            ExitCode = activeProcess.ExitCode;

            if (psi.RedirectStandardError)
            {
                thread_ReadStandardError.Join();
            }
            if (psi.RedirectStandardOutput)
            {
                thread_ReadStandardOut.Join();
            }

			string output = StdOut + StdErr;

			return output;
		}

        private Process? activeProcess = null;

        private void Thread_ReadStandardError()
        {
            if (activeProcess != null)
            {
                StdErr = activeProcess.StandardError.ReadToEnd();
            }
        }

        private void Thread_ReadStandardOut()
        {
            if (activeProcess != null)
            {
                StdOut = activeProcess.StandardOutput.ReadToEnd();
            }
        }

    }
}
