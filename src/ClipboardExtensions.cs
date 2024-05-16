using System.Runtime.InteropServices;

namespace KnowledgeClip;
public static class ClipboardExtensions
{
    public static void CopyToClipboard(this string text)
    {
        OpenClipboard(IntPtr.Zero);
        EmptyClipboard();
        IntPtr hGlobal = Marshal.StringToHGlobalUni(text);
        SetClipboardData(CF_UNICODETEXT, hGlobal);
        CloseClipboard();
        Marshal.FreeHGlobal(hGlobal);
    }

    private const uint CF_UNICODETEXT = 13;

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool OpenClipboard(IntPtr hWndNewOwner);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool EmptyClipboard();

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool CloseClipboard();
}