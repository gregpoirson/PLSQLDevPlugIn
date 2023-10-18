using RGiesecke.DllExport;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DemoPluginNet
{
    //delegate void IdeCreateWindow(int windowType, string text, [MarshalAs(UnmanagedType.Bool)] bool execute);

    //[return: MarshalAs(UnmanagedType.Bool)]
    //delegate bool IdeSetText(string text);

    //delegate IntPtr IdeGetSelectedText();

    delegate IntPtr IdeGetEditorHandle();

    public class DemoPlugin
    {
        [DllImport("user32.dll", EntryPoint = "SendMessageW")]
        public static extern int SendMessageW([In] IntPtr hWnd, int Msg, int wParam, IntPtr lParam);
        //public const int WM_SETTEXT = 0x000c;
        //public const int WM_GETTEXT = 0x000d;
        public const int EM_REPLACESEL = 0x00C2;
        //public const int EM_GETSEL = 0x00B0;
        //public const int EM_SETSEL = 0x00B1;
        //public const int WM_PASTE = 0x0302;

        private const string PLUGIN_NAME = "Align Results";
        private const string PLUGIN_NAME_2 = "Plug-in by GregPoirson";
        private const int PLUGIN_MENU_INDEX = 10;

        private const int CREATE_WINDOW_CALLBACK = 20;
        private const int SET_TEXT_CALLBACK = 34;
        private const int GET_SELECTED_TEXT_CALLBACK = 31;
        private const int GET_EDITOR_HANDLE = 33;

        private static DemoPlugin me;

        //private static IdeCreateWindow createWindowCallback;
        //private static IdeSetText setTextCallback;
        //private static IdeGetSelectedText getSelectedTextCallback;
        private static IdeGetEditorHandle getEditorHandleCallback;

        private int pluginId;

        private DemoPlugin(int id)
        {
            pluginId = id;
        }

        #region DLL exported API
        [DllExport("IdentifyPlugIn", CallingConvention = CallingConvention.Cdecl)]
        public static string IdentifyPlugIn(int id)
        {
            if (me == null)
            {
                me = new DemoPlugin(id);
            }
            return PLUGIN_NAME_2;
        }

        [DllExport("RegisterCallback", CallingConvention = CallingConvention.Cdecl)]
        public static void RegisterCallback(int index, IntPtr function)
        {
            switch (index)
            {
                //case CREATE_WINDOW_CALLBACK:
                //    createWindowCallback = (IdeCreateWindow)Marshal.GetDelegateForFunctionPointer(function, typeof(IdeCreateWindow));
                //    break;
                //case SET_TEXT_CALLBACK:
                //    setTextCallback = (IdeSetText)Marshal.GetDelegateForFunctionPointer(function, typeof(IdeSetText));
                //    break;
                //case GET_SELECTED_TEXT_CALLBACK:
                //    getSelectedTextCallback = (IdeGetSelectedText)Marshal.GetDelegateForFunctionPointer(function, typeof(IdeGetSelectedText));
                //    break;
                case GET_EDITOR_HANDLE:
                    getEditorHandleCallback = (IdeGetEditorHandle)Marshal.GetDelegateForFunctionPointer(function, typeof(IdeGetEditorHandle));
                    break;
            }
        }

        [DllExport("OnMenuClick", CallingConvention = CallingConvention.Cdecl)]
        public static void OnMenuClick(int index)
        {
            if (index == PLUGIN_MENU_INDEX)
            {
                var hwn = getEditorHandleCallback();

                me.InserirTextoFormatado(hwn);
            }
        }

        [DllExport("CreateMenuItem", CallingConvention = CallingConvention.Cdecl)]
        public static string CreateMenuItem(int index)
        {
            if (index == PLUGIN_MENU_INDEX)
            {
                return "Tools / " + PLUGIN_NAME; // "Tools / Demo plug-in in C#";
            }
            else
            {
                return "";
            }
        }

        [DllExport("About", CallingConvention = CallingConvention.Cdecl)]
        public static string About()
        {
            return PLUGIN_NAME_2;
        }
        #endregion

        //public string Name
        //{
        //    get
        //    {
        //        return PLUGIN_NAME;
        //    }

        //}

        private void InserirTextoFormatado(IntPtr handle)
        {
            var textToFormat = Clipboard.GetText();

            if (string.IsNullOrEmpty(textToFormat))
            {
                return;
            }

            var textoformatado = FormatarTabela.FormatarTextoTabela(textToFormat);

            //Clipboard.SetText(textoformatado);
            //me.PasteText(handle, textoformatado);

            if (string.IsNullOrEmpty(textoformatado))
            {
                textoformatado = "-- nada para formatar...";
            }

            ReplaceSelectedText(handle, textoformatado);
        }

        private void ReplaceSelectedText(IntPtr handle, string text)
        {
            var ptr = Marshal.StringToBSTR(text);
            SendMessageW(handle, EM_REPLACESEL, 1, ptr);

            Marshal.FreeBSTR(ptr);
        }

        //private void PasteText(IntPtr handle, string text)
        //{
        //    var ptr = Marshal.StringToBSTR(text);
        //    SendMessageW(handle, WM_PASTE, 0, IntPtr.Zero);

        //    //SendMessageW(handle, WM_SETTEXT, 0, ptr);

        //    Marshal.FreeBSTR(ptr);
        //}


        ////Get the text of a control with its handle
        //private string GetText(IntPtr handle)
        //{
        //    int maxLength = 10000;
        //    IntPtr buffer = Marshal.AllocHGlobal((maxLength + 1) * 2);
        //    SendMessageW(handle, WM_GETTEXT, maxLength, buffer);
        //    string w = Marshal.PtrToStringUni(buffer);
        //    Marshal.FreeHGlobal(buffer);
        //    return w;
        //}

    }
}
