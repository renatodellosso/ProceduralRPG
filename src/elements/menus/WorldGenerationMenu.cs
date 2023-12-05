using MenuEngine.src;
using MenuEngine.src.elements;
using ProceduralRPG.src.world.generation;
using System.Collections.Generic;
using System.Diagnostics;

namespace ProceduralRPG.src.elements.menus
{
    internal class WorldGenerationMenu : Element
    {

        internal TextElement titleElement;

        private List<string> log;
        private const int LOG_LENGTH = 25;
        private TextElement logElement;

        internal WorldGenerationMenu(WorldGenerationSettings settings)
        {
            log = new();

            titleElement = new TextElement(this, new(0.35f, 0.1f), new(0.3f, 0.05f), "World Generation in Progress...",
                justify: TextElement.Justify.Center, align: TextElement.Align.Center);
            _ = new ButtonElement(this, new(0.2f, 0.1f), new(0.075f, 0.05f), labelText: "Cancel", onClick: Cancel);
            logElement = new TextElement(this, new(0.1f, 0.2f), new(0.8f, 0.7f), "", justify: TextElement.Justify.Left, align: TextElement.Align.Bottom);

            WorldGenerator.Start(this, settings);

        }

        internal void Cancel()
        {
            _ = new TitleScreenMenu();
            Dispose();
        }

        internal void Log(string message)
        {
            Debug.WriteLine(message);

            log.Add(message);

            if (log.Count > LOG_LENGTH)
                log.RemoveAt(0);

            string logString = "";
            foreach (string logMessage in log)
                logString += logMessage + "\n";

            logElement.SetText(logString);
        }

    }
}
