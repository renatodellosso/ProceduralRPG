using MenuEngine.src;
using MenuEngine.src.elements;
using ProceduralRPG.src.worldgeneration;

namespace ProceduralRPG.src.elements.menus
{
    internal class NewWorldMenu : Element
    {

        private WorldGenerationSettings settings;

        internal NewWorldMenu()
        {
            settings = new();

            _ = new TextElement(this, new(0.45f, 0.1f), new(0.1f, 0.05f), "World Generation", justify: TextElement.Justify.Center, align: TextElement.Align.Center);
            _ = new ButtonElement(this, new(0.2f, 0.1f), new(0.075f, 0.05f), labelText: "Back", onClick: Back);
            _ = new ButtonElement(this, new(0.45f, 0.85f), new(0.1f, 0.05f), labelText: "Generate", onClick: Generate);
        }

        internal void Back()
        {
            _ = new TitleScreenMenu();
            Dispose();
        }

        internal void Generate()
        {
            _ = new WorldGenerationMenu(settings);
            Dispose();
        }

    }
}
