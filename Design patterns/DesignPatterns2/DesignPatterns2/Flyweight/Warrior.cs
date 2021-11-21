using System;
namespace DesignPatterns2.Flyweight
{
    public class Warrior : Character
    {
        // Constructor
        public Warrior()
        {
            HealthPoints = 70;
        }
        public override void Display(int damage)
        {
            this.Damage = damage;
            string label = $"The {this.GetType().Name} has {HealthPoints} health points and {this.Damage} of damage";
            Console.WriteLine(label);
        }
    }
}
