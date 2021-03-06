﻿using System;
using Xunit;
using Xunit.Abstractions;


namespace GameEngine.Tests
{
    public class EnemyFactoryShould
    {
        private readonly ITestOutputHelper _output;

        public EnemyFactoryShould(ITestOutputHelper output)
        {
            this._output = output;
        }

        [Fact]
        public void CreateNormalEnemy()
        {
            EnemyFactory sut = new EnemyFactory();
            _output.WriteLine("Creating an enemy and validating type");
            Enemy enemy = sut.Create("Zombie");
            Assert.IsType<NormalEnemy>(enemy);
        }

        [Fact]
        public void CreateNormalEnemy_NotTypeExample()
        {
            EnemyFactory sut = new EnemyFactory();
            Enemy enemy = sut.Create("Zombie");
            Assert.IsNotType<DateTime>(enemy);
        }

        [Fact]
        public void CreateBossEnemy()
        {
            EnemyFactory sut = new EnemyFactory();
            Enemy enemy = sut.Create("Zombie King", true);
            Assert.IsType<BossEnemy>(enemy);
        }

        [Fact]
        public void CreateBossEnemy_NonStrict()
        {
            EnemyFactory sut = new EnemyFactory();
            Enemy enemy = sut.Create("Zombie King", true);
            Assert.IsAssignableFrom<Enemy>(enemy);
        }

        [Fact]
        public void CreateSeparateInstances()
        {
            EnemyFactory sut = new EnemyFactory();
            Enemy enemy1 = sut.Create("Zombie");
            Enemy enemy2 = sut.Create("Zombie");

            Assert.NotSame(enemy1, enemy2);
        }

        [Fact]
        public void NotAllowNullName()
        {
            EnemyFactory sut = new EnemyFactory();

            //Assert.Throws<ArgumentNullException>(() => sut.Create(null));

            //You can also pass the argument that raises the exception
            Assert.Throws<ArgumentNullException>("name", () => sut.Create(null));
        }

        [Fact(Skip = "Don't need to run this")]
        public void OnlyAllowKingOrQueenBosses()
        {
            EnemyFactory sut = new EnemyFactory();

            EnemyCreationException ex = Assert.Throws<EnemyCreationException>(() => sut.Create("Zombie", true));

            Assert.Equal("Zombie", ex.RequestedEnemyName);
        }

    }
}
