using System;
using System.Text;

namespace TestEmail
{
    /// <summary>
    /// Helper class for default exception messages
    /// </summary>
    public static class ExceptionHelper
    {
        /// <summary>
        /// Get exception type and message for a given exception, including
        /// inner exception's type and message
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetMessages(Exception ex)
        {
            return GetMessages(ex, null);
        }

        /// <summary>
        /// Get exception type and message for a given exception, including
        /// inner exception's type and message. Also include sender'a name
        /// when available.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="sender"></param>
        /// <returns></returns>
        public static string GetMessages(Exception ex, object sender)
        {
            if (ex == null)
                throw new ArgumentNullException("ex");

            StringBuilder sb = new StringBuilder(200);

            // Exception Source
            if (sender != null)
            {
                sb.AppendLine(sender.GetType().FullName);
            }

            // Exception Type
            try
            {
                sb.AppendLine(ex.GetType().FullName);
            }
            catch (Exception e)
            {
                // avoid any unhandled exception here
                sb.AppendLine(e.Message);
            }

            // Exception message
            try
            {
                sb.AppendLine(ex.Message);
            }
            catch (Exception e)
            {
                // avoid any unhandled exception here
                sb.AppendLine(e.Message);
            }

            if (ex.InnerException != null)
            {
                sb.AppendLine();
                sb.AppendLine(GetMessages(ex.InnerException));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Get exception type and message for a given exception, including
        /// inner exception's type and message. Also include sender'a name
        /// and a custom message when available.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string GetMessages(Exception ex, object sender, string message)
        {
            if (ex == null)
                throw new ArgumentNullException("ex");

            StringBuilder sb = new StringBuilder(300);

            // extra message passed in
            if (!string.IsNullOrEmpty(message))
            {
                sb.AppendLine(message);
                sb.AppendLine();
            }

            // standard message including inner exception
            sb.Append(GetMessages(ex, sender));

            return sb.ToString();
        }

        /// <summary>
        /// Create a detailed report for a given exception, including
        /// inner exception and stack trace information
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetExceptionDetails(Exception ex)
        {
            if (ex == null)
                throw new ArgumentNullException("ex");

            StringBuilder sb = new StringBuilder(1000);

            sb.Append("Exception Source:      ");
            try
            {
                sb.Append(ex.Source);
            }
            catch (Exception e)
            {
                // avoid any unhandled exception here
                sb.Append(e.Message);
            }

            sb.Append(Environment.NewLine);
            sb.Append("Exception Type:        ");
            try
            {
                sb.Append(ex.GetType().FullName);
            }
            catch (Exception e)
            {
                // avoid any unhandled exception here
                sb.Append(e.Message);
            }

            sb.Append(Environment.NewLine);
            sb.Append("Exception Message:     ");
            try
            {
                sb.Append(ex.Message);
            }
            catch (Exception e)
            {
                // avoid any unhandled exception here
                sb.Append(e.Message);
            }

            sb.Append(Environment.NewLine);
            sb.Append("Exception Target Site: ");
            try
            {
                sb.Append(ex.TargetSite.Name);
            }
            catch (Exception e)
            {
                // avoid any unhandled exception here
                sb.Append(e.Message);
            }

            sb.Append(Environment.NewLine);
            sb.Append("---- Stack Trace ----");
            sb.Append(Environment.NewLine);
            try
            {
                sb.Append(ex.StackTrace);
            }
            catch (Exception e)
            {
                // avoid any unhandled exception here
                sb.Append(e.Message);
            }

            sb.Append(Environment.NewLine);

            if (ex.InnerException != null)
            {
                sb.AppendLine();
                sb.AppendLine("---Inner Exception---");
                sb.AppendLine(GetExceptionDetails(ex.InnerException));
            }

            return sb.ToString();
        }

    

      

    }
}
