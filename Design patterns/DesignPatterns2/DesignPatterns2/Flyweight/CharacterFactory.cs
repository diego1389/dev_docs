using System.Collections.Generic;

namespace DesignPatterns2.Flyweight
{
    public class CharacterFactory
    {
        private Dictionary<CharacterTypes, Character> characters = new Dictionary<CharacterTypes, Character>();
        public Character GetCharacter(CharacterTypes key)
        {
            // Uses "lazy initialization"
            Character character = null;
            if (characters.ContainsKey(key))
            {
                character = characters[key];
            }
            else
            {
                switch (key)
                {
                    case CharacterTypes.Wizard: character = new Wizard(); break;
                    case CharacterTypes.Warrior: character = new Warrior(); break;
                    case CharacterTypes.Healer: character = new Healer(); break;
                }
                characters.Add(key, character);
            }
            return character;
        }
    }
}
