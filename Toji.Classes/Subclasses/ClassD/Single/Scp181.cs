using Toji.Classes.API.Interfaces;

namespace Toji.Classes.Subclasses.ClassD.Single
{
    public class Scp181 : DSingleSubclass, IHintSubclass, ICustomHintSubclass, IRandomSubclass
    {
        public override bool ShowInfo => true;

        public override string Name => "SCP-181";

        public override string Desc => "Тебе настолько сильно везет, что тебя записали как аномальный SCP-объект";

        public string HintText => string.Empty;

        public string HintColor => "#009A63";

        public float HintDuration => 15;

        public int Chance => 4;
    }
}
