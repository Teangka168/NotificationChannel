using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text.RegularExpressions;
using System.Data;
using System.Windows.Forms;

namespace NotificationChannelParser
{
    class NotificationCP
    {


        private static DataTable _dtMessage = null;
        private static string[] _strChannel = { "BE", "FE", "QA", "Urgent" };

        static void Main()
        {
            
            for (; ; )
            {
                Console.Clear();
                showNameProject();
                outputStr("1", "Backend (BE)");
                outputStr("2", "Frontend (FE)");
                outputStr("3", "Quality Assurance (QA)");
                outputStr("4", "Urgent (Urgent)");
                outputStr("5", "Exit");

                string strOption = Console.ReadLine();
           
                
                if (strOption == "1")
                {
                    showChannel("BE");
                }
                else if (strOption == "2")
                {
                    showChannel("FE");
                }
                else if (strOption == "3")
                {
                    showChannel("QA");
                }
                else if (strOption == "4")
                {
                    showChannel("Urgent");
                }
                else if (strOption == "5")
                {
                    vExit();
                }
                else
                {
                    Console.WriteLine("Error! Please choose a valid option..!");
                    Thread.Sleep(2000);
                }
            }
        }

        private static void outputStr(string value, string message)
        {
            Console.Write("[");
            Console.Write(value, ConsoleColor.DarkBlue);
            Console.WriteLine("]" + message);
        }

        private static void showChannel(string strChannel)
        {
            Console.Clear();
            showNameProject();
            outputStr("1", "Write Message");
            outputStr("2", "Read Message");
            outputStr("3", "Back to Menu");

            alertNotification(strChannel);

            string strOption = Console.ReadLine();
          
            if (strOption == "1")
            {
                vWriteMessage(strChannel);
                showChannel(strChannel);
            }
            else if (strOption == "2")
            {
                vReadMessage(strChannel);                
                showChannel(strChannel);
            }
            else if (strOption == "3")
            {
                Main();
            }
            else
            {
                Console.WriteLine("Error! Please choose a valid option..!");
                Thread.Sleep(2000);
                showChannel(strChannel);
            }


        }

        private static void vWriteMessage(string strChannel)
        {
            Console.Clear();
            showNameProject();
            Console.Write("Titles: ");
            string strTitles = Console.ReadLine();

            Console.Write("Message: ");
            string strMessage = Console.ReadLine();

            //int i = _strMessage.Length;


            ICollection<string> matches = Regex.Matches(strTitles.Replace(Environment.NewLine, ""), @"\[([^]]*)\]")
                                            .Cast<Match>()
                                            .Select(x => x.Groups[1].Value)
                                            .ToList();
            int i = 1;
            if (_dtMessage==null)
            {
                _dtMessage = setTableStructure();
            }
            else
            {
                i = _dtMessage.Rows.Count + 1;
            }

            foreach (string match in matches)
            {
                foreach (string strCN in _strChannel)
                {
                    if (strCN == match)
                    {
                        DataRow dr = _dtMessage.NewRow();
                        dr[0] = i;
                        dr[1] = strTitles;
                        dr[2] = strMessage;
                        dr[3] = false;
                        dr[4] = match;
                        _dtMessage.Rows.Add(dr);
                        i++;
                    }
                }

            }


        }

        private static void vReadMessage(string strChannel)
        {
            Console.Clear();
            showNameProject();
            if (_dtMessage != null)
            {
                if (_dtMessage.Rows.Count != 0)
                {
                    for (int i = 0; i < _dtMessage.Rows.Count; i++)
                    {
                        if (_dtMessage.Rows[i]["channel"].ToString() == strChannel)
                        {
                            Console.WriteLine("Titles: " + _dtMessage.Rows[i]["titles"].ToString());
                            Console.WriteLine("Message: " + _dtMessage.Rows[i]["message"].ToString());
                            Console.WriteLine("------------------------------------------");
                            _dtMessage.Rows[i]["is_read"] = true;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No Message to show..!");
                }
            }
            else
            {
                Console.WriteLine("No Message to show..!");
            }
            Console.WriteLine("-----------Please Enter to back-------------");
            Console.ReadLine();
        }

        private static void alertNotification(string strChannel)
        {
            if (_dtMessage != null)
            {
                if (_dtMessage.Rows.Count != 0)
                {
                    DataRow[] dtTemp = _dtMessage.Select("channel='" + strChannel + "' and is_read='False'");
                    if (dtTemp.Length > 0)
                    {
                        int intMessageCount = dtTemp.Length;
                        string strMessageShow = "";
                        foreach (DataRow drRow in dtTemp)
                        {
                            strMessageShow = strMessageShow + "Title: " + drRow["titles"].ToString() + "\n";
                            //if (bool.Parse(drRow["is_read"].ToString()))
                            //{
                            //    MessageBox.Show("This Channel Have New Message..!" + "\n" + "Title: " + drRow["titles"].ToString());
                            //}
                        }
                        MessageBox.Show(strMessageShow, "You Have " + intMessageCount + " New Message..!");
                    }
                }
            }
        }

        private static void vExit()
        {
            Environment.Exit(0);
            //string s = "test [4df] test [5y" + Environment.NewLine + "u] test [6nf]";

            //ICollection<string> matches =
            //    Regex.Matches(s.Replace(Environment.NewLine, ""), @"\[([^]]*)\]")
            //        .Cast<Match>()
            //        .Select(x => x.Groups[1].Value)
            //        .ToList();

            //foreach (string match in matches)
            //    Console.WriteLine(match);
        }

        private static DataTable setTableStructure()
        {
            DataTable dtReturn = new DataTable();
            dtReturn.Columns.Add("id", typeof(int));
            dtReturn.Columns.Add("titles", typeof(string));
            dtReturn.Columns.Add("message", typeof(string));
            dtReturn.Columns.Add("is_read", typeof(bool));
            dtReturn.Columns.Add("channel", typeof(string));
            return dtReturn;
        }

        private static void showNameProject()
        {

            string strNameProject = "MESSAGE AUTO SEND BY TAGS";

            Console.WriteLine("-------------------------------------------------------------------");
            Console.WriteLine(strNameProject);
            Console.WriteLine("-------------------------------------------------------------------");
        }




    }
}
