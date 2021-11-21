namespace DesignPatterns2.Flyweight
{
    public abstract class Character
    {
        public int HealthPoints { get; set; }
        public int Damage { get; set; }
        public abstract void Display(int damage);
    }
}
