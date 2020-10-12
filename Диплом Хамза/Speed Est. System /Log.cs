using System.Windows.Forms;

namespace Diplom
{
    public static class Log
    {
        public static void AddToLog(string entry)
        {
            if (Form1.ActiveForm != null)
            {
                var form = Form1.ActiveForm;

                foreach (var cont in form.Controls)
                {
                    if (cont.GetType() == typeof(SplitContainer))
                    {
                        var scont = (SplitContainer)cont;

                        foreach (var cont1 in scont.Panel1.Controls)
                        {
                            if (cont1.GetType() == typeof(TextBox))
                            {
                                var tb = (TextBox)cont1;

                                tb.Text += entry + "\r\n\r\n";
                            }
                        }

                        break;
                    }
                }
            }
        }

        public static void AddToLog(string entry, Form1 form)
        {

            foreach (var cont in form.Controls)
            {
                if (cont.GetType() == typeof(SplitContainer))
                {
                    var scont = (SplitContainer)cont;

                    foreach (var cont1 in scont.Panel1.Controls)
                    {
                        if (cont1.GetType() == typeof(TextBox))
                        {
                            var tb = (TextBox)cont1;

                            tb.Text += entry + "\r\n\r\n";
                        }
                    }

                    break;
                }
            }
        }
    }
}
