using System;
using System.Collections;
using System.ComponentModel;

namespace kasthack.Tools
{
    public static class ConTools {

        /// <summary>
        /// Alias for Console.WriteLine(t). If passed argument is IEnumerable function also prints all members
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public static void Dump<T>( this T t ) {
            Console.WriteLine( t.ToString() );
            if ( !( t is IEnumerable )||(t is string) ) return;
            foreach ( var v in t as IEnumerable )
                if ( v != null )
                    v.Dump();
        }

        /// <summary>
        /// Python like ReadLine with prompt
        /// </summary>
        /// <param name="str">Text to show</param>
        /// <param name="prompt">Prompt string(default - ": ")</param>
        /// <param name="bydefault">Value returned if user have just pressed enter</param>
        /// <param name="foreColor">Text color(default - Yellow)</param>
        /// <param name="backColor">BG color(default - Black)></param>
        /// <returns></returns>
        public static string ReadLine( string str, object bydefault = null, string prompt = ": ", ConsoleColor foreColor = ConsoleColor.Yellow, ConsoleColor backColor = ConsoleColor.Black ) {
            var msg = bydefault == null ?
                string.Format( "{0} {1}", str, prompt ) :
                string.Format( "{0} [{2}]{1}", str, prompt, bydefault );
            ColorWrite( msg, foreColor, backColor );
            var s = Console.ReadLine();
            if ( s == String.Empty && bydefault != null )
                return bydefault.ToString();
            return s;
        }

        /// <summary>
        /// Python like ReadLine with prompt
        /// </summary>
        /// <param name="str">Text to show</param>
        /// <param name="prompt">Prompt string(default - ": ")</param>
        /// <param name="bydefault">Return value if empty</param>
        /// <param name="foreColor">Text color(default - Yellow)</param>
        /// <param name="backColor">BG color(default - Black)></param>
        /// <exception cref="FormatException">thrown by int.Parse</exception>
        /// <returns></returns>
        public static int ReadInt( string str, int? bydefault = null, string prompt = ": ", ConsoleColor foreColor = ConsoleColor.Yellow, ConsoleColor backColor = ConsoleColor.Black ) {
            return int.Parse( ReadLine( str, bydefault, prompt, foreColor, backColor ).Trim() );
        }

        /// <summary>
        /// Read lines from console until user will input valid data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str">Text to show</param>
        /// <param name="validator">validator</param>
        /// <param name="errorMessage">message shown if it's impossible to parse T from input or validator returned false</param>
        /// <param name="prompt">Prompt string(default - ": ")</param>
        /// <returns>parsed value</returns>
        public static T ReadValidT<T>( string str, Func<T, bool> validator = null, string errorMessage = "You entered bad value", string prompt = ": ", ConsoleColor foreColor = ConsoleColor.Yellow, ConsoleColor backColor = ConsoleColor.Black ) {
            if ( validator == null )
                validator = a => true;
            var type = typeof( T );
            var converter = TypeDescriptor.GetConverter( type );
            if ( converter == null )
                throw new FormatException( "Can't find TryParse method for" );
            while ( true ) {
                try {
                    ColorWrite( str + prompt, foreColor, backColor );
                    var t = (T) converter.ConvertFromString( Console.ReadLine() );
                    if ( validator( t ) )
                        return t;
                }
                catch { }
                WriteError( errorMessage );
            }
        }

        /// <summary>
        /// Write colored text & restore colors if needed
        /// </summary>
        /// <param name="str">Text to show</param>
        /// <param name="foreColor">Text color</param>
        /// <param name="backColor">BG color</param>
        /// <param name="newline">Append newline to text</param>
        public static void ColorWrite( string str, ConsoleColor foreColor, ConsoleColor backColor, bool newline = false ) {
            var f = Console.ForegroundColor;
            var b = Console.BackgroundColor;
            if ( f != foreColor )
                Console.ForegroundColor = foreColor;
            if ( b != backColor )
                Console.BackgroundColor = backColor;

            Console.Write( str );
            if ( newline )
                Console.WriteLine();

            if ( f != foreColor )
                Console.ForegroundColor = f;
            if ( b != backColor )
                Console.BackgroundColor = b;
        }

        /// <summary>
        /// Alias for ColorWrite with newline
        /// </summary>
        /// <param name="str">Text to show</param>
        /// <param name="foreColor">Text color</param>
        /// <param name="backColor">BG color</param>
        public static void ColorWriteLine( string str, ConsoleColor foreColor, ConsoleColor backColor ) {
            ColorWrite( str, foreColor, backColor, true );
        }

        /// <summary>
        /// Alias for ColorWrite( str, Gray, Black )
        /// </summary>
        /// <param name="str">Text to show</param>
        /// <param name="newline">Append newline to text</param>
        public static void WriteMessage( string str, bool newline = true ) {
            ColorWrite( str, ConsoleColor.Gray, ConsoleColor.Black, newline );
        }
        /// <summary>
        /// Alias for ColorWrite( str, Red, Black )
        /// </summary>
        /// <param name="str">Text to show</param>
        /// <param name="newline">Append newline to text</param>
        public static void WriteError( string str, bool newline = true ) {
            ColorWrite( str, ConsoleColor.Red, ConsoleColor.Black, newline );
        }

        /// <summary>
        /// Alias for ColorWrite( str, Red, Black )
        /// </summary>
        /// <param name="str">Text to show</param>
        /// <param name="prompt">Prompt string(default - ": ")</param>
        /// <param name="newline">Append newline to text</param>
        public static void WriteQuestion( string str, string prompt = ": ", bool newline = false ) {
            ColorWrite( str + prompt, ConsoleColor.Yellow, ConsoleColor.Black, newline );
        }
    }

}
