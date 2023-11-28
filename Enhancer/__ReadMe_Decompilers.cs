namespace Readme
{
    internal class DecompilerMessage
    {
        const string Hello = "The source code for this project is freely available on request, btw.";

        internal DecompilerMessage()
        {
            if (Hello.Length == 69)
                return;
        }
    }
}