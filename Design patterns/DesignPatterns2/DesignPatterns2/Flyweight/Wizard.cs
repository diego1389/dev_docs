using System;
namespace DesignPatterns2.Flyweight
{
    public class Wizard : Character
    {
        // Constructor
        public Wizard()
        {
            HealthPoints = 50;
        }
        public override void Display(int damage)
        {
            this.Damage = damage;
            string label = $"The {this.GetType().Name} has {HealthPoints} health points and {this.Damage} of damage";
            Console.WriteLine(label);
        }
    }
}
