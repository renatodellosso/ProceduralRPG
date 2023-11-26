using MenuEngine.src;
using ProceduralRPG.src.elements.menus;

namespace ProceduralRPG.src
{
    internal class MyProject : Project
    {

        private TitleScreenMenu titleScreen;

        public override void OnInitialize()
        {
            titleScreen = new();
        }
    }
}