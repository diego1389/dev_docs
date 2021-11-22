using System;
namespace DesignPatterns2.FactoryMethod
{
    public class Resume : Document
    {
        public override void CreatePages()
        {
            Pages.Add(new SkillsPage());
            Pages.Add(new EducationPage());
        }
    }
}
