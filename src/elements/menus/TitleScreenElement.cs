using MenuEngine.src;
using MenuEngine.src.elements;

namespace ProceduralRPG.src.elements.menus
{
    internal class TitleScreenMenu : Element
    {

        internal TitleScreenMenu()
        {
            _ = new TextElement(this, new(0.25f, 0.2f), new(0.5f, 0.3f), "Procedural RPG", justify: TextElement.Justify.Center, align: TextElement.Align.Center);
            _ = new ButtonElement(this, new(0.5f - 0.25f / 2, 0.45f), new(0.25f, 0.05f), labelText: "Continue", onClick: Continue);
            _ = new ButtonElement(this, new(0.5f - 0.25f / 2, 0.55f), new(0.25f, 0.05f), labelText: "New World", onClick: NewWorld);
            _ = new ButtonElement(this, new(0.5f - 0.25f / 2, 0.65f), new(0.25f, 0.05f), labelText: "Load World", onClick: LoadWorld);
            _ = new ButtonElement(this, new(0.5f - 0.25f / 2, 0.75f), new(0.25f, 0.05f), labelText: "Exit", onClick: Exit);
        }

        private void NewWorld()
        {
            _ = new NewWorldMenu();
            Dispose();
        }

        private void LoadWorld()
        {

        }

        private void Continue()
        {

        }

        private void Exit()
        {
            Engine.Quit();
        }
    }
}
