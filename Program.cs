using System;
using System.Threading;
using System.Collections.Generic;

class Program
{
    class Weapon
    {
        public string Name { get; set; }
        public int Damage { get; set; }
        public int Durability { get; set; }
        public int MaxDurability { get; set; }

        public Weapon(string name, int damage, int durability)
        {
            Name = name;
            Damage = damage;
            Durability = durability;
            MaxDurability = durability;
        }
    }

    class Aid
    {
        public string Name { get; set; }
        public int HealPower { get; set; }

        public Aid(string name, int healPower)
        {
            Name = name;
            HealPower = healPower;
        }
    }

    class Player
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public Weapon CurrentWeapon { get; set; }
        public List<Weapon> Weapons { get; set; }
        public Aid FirstAidKit { get; set; }
        public int Score { get; set; }

        public Player(string name)
        {
            Name = name;
            MaxHealth = 100;
            Health = MaxHealth;
            Score = 0;
            Weapons = new List<Weapon>();
        }

        public void Attack(Enemy enemy)
        {
            enemy.Health -= CurrentWeapon.Damage;
            CurrentWeapon.Durability--;
        }

        public void Heal()
        {
            Health = Math.Min(MaxHealth, Health + FirstAidKit.HealPower);
        }
    }

    class Enemy
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public Weapon Weapon { get; set; }
        public int Points { get; set; }

        public Enemy(string name, int health, Weapon weapon, int points)
        {
            Name = name;
            MaxHealth = health;
            Health = MaxHealth;
            Weapon = weapon;
            Points = points;
        }

        public void Attack(Player player)
        {
            player.Health -= Weapon.Damage;
        }
    }

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // ЗАГОЛОВОК )))
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine(@"
         ███████████                                                     ███           
        ░░███░░░░░███                                                   ░░░            
         ░███    ░███  ██████   ████████   ██████   ████████    ██████  ████   ██████  
         ░██████████  ░░░░░███ ░░███░░███ ░░░░░███ ░░███░░███  ███░░███░░███  ░░░░░███ 
         ░███░░░░░░    ███████  ░███ ░░░   ███████  ░███ ░███ ░███ ░███ ░███   ███████ 
         ░███         ███░░███  ░███      ███░░███  ░███ ░███ ░███ ░███ ░███  ███░░███ 
         █████       ░░████████ █████    ░░████████ ████ █████░░██████  █████░░████████
        ░░░░░         ░░░░░░░░ ░░░░░      ░░░░░░░░ ░░░░ ░░░░░  ░░░░░░  ░░░░░  ░░░░░░░░ ");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("\nДобро пожаловать в подземелье ПАРАНОЙЯ!");
        Console.Write("\nКто ты войн?: ");
        string name = Console.ReadLine();

        Player player = new Player(name)
        {
            FirstAidKit = new Aid("Аптечка", 33)
        };

        // НАЧАЛЬНОЕ ОРУЖИЕ
        Weapon startWeapon = new Weapon("Ржавый меч", 10, 15);
        player.CurrentWeapon = startWeapon;
        player.Weapons.Add(startWeapon);

        // bOoss
        Enemy[] bosses = {
            new Enemy("Смерть-Маг", 80, new Weapon("Темная магия", 15, 999), 100),
            new Enemy("СКЕЛЕТ", 100, new Weapon("Топор", 18, 999), 150),
            new Enemy("ОГР", 130, new Weapon("Железная булава", 20, 999), 200),
            new Enemy("Дракон", 150, new Weapon("Огненное дыхание", 25, 999), 300),
            new Enemy("Паук", 180, new Weapon("Паучия кислота", 27, 999), 400)
        };

        Weapon[] rewards = {
            new Weapon("Волшебный скипетр", 20, 25),
            new Weapon("ТОПОР", 30, 30),
            new Weapon("Железная булава", 35, 30),
            new Weapon("Драконий меч", 40, 35),
            new Weapon("Паучий клинок", 45, 40)
        };

        // ВХОД В ПЕЩЕРУ 
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("\nВы входите в темную пещеру...");
        Console.WriteLine(@"
                                                           ,--.
                                                          {    }
                                                          K,   }
                                                         /  `Y`
                                                    _   /   /
                                                   {_'-K.__/
                                                     `/-.__L._
                                                     /  ' /`\_}
                                                    /  ' /     
                                            ____   /  ' /
                                     ,-'~~~~    ~~/  ' /_
                                   ,'             ``~~~%%',
                                  (                     %  Y
                                 {                      %% I
                                {      -                 %  `.
                                |       ',                %  )
                                |        |   ,..__      __. Y
                                |    .,_./  Y ' / ^Y   J   )|
                                \           |' /   |   |   ||
                                 \          L_/    . _ (_,.'(
                                  \,   ,      ^^""' / |      )
                                    \_  \          /,L]     /
                                      '-_`-,       ` `   ./`
                                         `-(_            )
                                             ^^\..___,.--` ");

        Console.WriteLine("Перед вами груда костей и черепов.");
        Console.WriteLine("Похоже, многие искатели приключений не вернулись обратно. . .");
        Console.ResetColor();
        Continue();

        // ИСПРАВЛЕННЫЙ ЦИКЛ - используем bosses вместо originalBosses
        for (int i = 0; i < bosses.Length; i++)
        {
            bool win = StartBossBattle(player, bosses[i], i);
            if (!win)
            {
                GameOver(player);
                return;
            }

            // weapon
            player.Weapons.Add(rewards[i]);
            player.CurrentWeapon = rewards[i];
            player.Score += bosses[i].Points;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n  +{bosses[i].Points} очков! Всего: {player.Score}");
            Console.WriteLine($"  Новое оружие: {rewards[i].Name}");
            Console.ResetColor();

            if (i < bosses.Length - 1) Continue();
        }

        Victory(player);
    }

    // боссы
    static bool StartBossBattle(Player player, Enemy enemy, int bossIndex)
    {
        ShowBossArt(bossIndex, enemy.Name);

        while (enemy.Health > 0 && player.Health > 0)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\n{player.Name}: {player.Health} HP | {enemy.Name}: {enemy.Health} HP");

            // об оружи
            string durabilityInfo = player.CurrentWeapon.Durability > 0 ?
                $"{player.CurrentWeapon.Durability}/{player.CurrentWeapon.MaxDurability}" : "∞";

            Console.WriteLine($" Оружие: {player.CurrentWeapon.Name} (Урон: {player.CurrentWeapon.Damage}, Прочность: {durabilityInfo})");
            Console.ResetColor();

            Console.WriteLine("\n1. Атаковать");
            Console.WriteLine("2. Использовать аптечку");
            Console.WriteLine("3. Сменить оружие");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    player.Attack(enemy);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($" Вы атаковали {enemy.Name} и нанесли {player.CurrentWeapon.Damage} урона!");

                    // ПРОВЕРКА ПРОЧНОСТИ
                    if (player.CurrentWeapon.Durability <= 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($" {player.CurrentWeapon.Name} СЛОМАЛСЯ! Вы в страхе бьете на кулаками");
                        player.CurrentWeapon = new Weapon("Кулаки", 5, 999);
                    }
                    else if (player.CurrentWeapon.Durability <= 3)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($" Оружие почти сломалось! Осталось {player.CurrentWeapon.Durability} ударов");
                    }
                    Console.ResetColor();
                    break;

                case "2":
                    player.Heal();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($" Вы использовали аптечку и восстановили {player.FirstAidKit.HealPower} HP!");
                    Console.ResetColor();
                    break;

                case "3":
                    ChangeWeapon(player);
                    break;

                default:
                    Console.WriteLine(" Неверный выбор! Пропускаем ход.");
                    break;
            }

            if (enemy.Health > 0)
            {
                enemy.Attack(player);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($" {enemy.Name} атаковал Вас и нанес {enemy.Weapon.Damage} урона!");
                Console.ResetColor();
            }
        }

        if (player.Health <= 0) return false;

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n Вы победили {enemy.Name}!");
        Console.ResetColor();
        return true;
    }

    // СМЕНА ОРУЖИЯ
    static void ChangeWeapon(Player player)
    {
        Console.WriteLine("\n ВАШЕ ВООРУЖЕНИЕ ");
        for (int i = 0; i < player.Weapons.Count; i++)
        {
            string durInfo = player.Weapons[i].Durability > 0 ?
                $"{player.Weapons[i].Durability}/{player.Weapons[i].MaxDurability}" : "∞";
            string current = player.Weapons[i].Name == player.CurrentWeapon.Name ? "(ТЕКУЩЕЕ)" : "";
            Console.WriteLine($"{i + 1}. {player.Weapons[i].Name} (Урон: {player.Weapons[i].Damage}, Прочность: {durInfo}) {current}");
        }

        Console.Write("Выберите оружие (номер): ");
        if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= player.Weapons.Count)
        {
            player.CurrentWeapon = player.Weapons[choice - 1];
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  Вы взяли {player.CurrentWeapon.Name}!");
            Console.ResetColor();
        }
        else
        {
            Console.WriteLine(" Неверный выбор!");
        }
    }

    // ПОКАЗ АРТОВ БОССОВ
    static void ShowBossArt(int bossIndex, string bossName)
    {
        Console.Clear();

        switch (bossIndex)
        {
            case 0: // МАГ
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(@"

              o
                   O       /`-.__
                          /  \'^|
             o           T    l  *
                        _|-..-|_
                 O    (^ '----' `)
                       `\-....-/^
             O       o  ) ""/ "" (
                       _( (-)  )_
                   O  /\ )    (  /\
                     /  \(    ) |  \
                 o  o    \)  ( /    \
                   /     |(  )|      \
                  /    o \ \( /       \
            __.--'   O    \_ /   .._   \
           //|)\      ,   (_)   /(((\^)'\
              |       | O         )  `  |
              |      / o___      /      /
             /  _.-''^^__O_^^''-._     /
           .'  /  -''^^    ^^''-  \--'^
         .'   .`.  `'''----'''^  .`. \
       .'    /   `'--..____..--'^   \ \
      /  _.-/                        \ \
  .::'_/^   |                        |  `.
         .-'|                        |    `-.
   _.--'`   \                        /       `-.
  /          \                      /           `-._
  `'---..__   `.                  .'_.._   __       \
           ``'''`.              .'gnv   `'^  `''---'^
                  `-..______..-' ");
                Console.WriteLine("\n ПЕРЕД ВАМИ ВСТАЕТ ТЕМНЫЙ МАГ - СМЕРТИ!");
                Console.ResetColor();
                break;

            case 1: // СКЕЛЕТ
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(@"

                                                                     _.--""""-._
                                          .                        .""         "".
                                         / \    ,^.         /(     Y             |      )\
                                        /   `---. |--'\    (  \__..'--   -   -- -'""""-.-'  )
                                        |        :|    `>   '.     l_..-------.._l      .'
                                        |      __l;__ .'     ""-.__.||_.-'v'-._||`""----""
                                         \  .-' | |  `              l._       _.'
                                          \/    | |                   l`^^'^^'j
                                                | |                _   \_____/     _
                                                j |               l `--__)-'(__.--' |
                                                | |               | /`---``-----'""1 |  ,-----.
                                                | |               )/  `--' '---'   \'-'  ___  `-.
                                                | |              //  `-'  '`----'  /  ,-'   I`.  \
                                              _ L |_            //  `-.-.'`-----' /  /  |   |  `. \
                                             '._' / \         _/(   `/   )- ---' ;  /__.J   L.__.\ :
                                              `._;/7(-.......'  /        ) (     |  |            | |
                                              `._;l _'--------_/        )-'/     :  |___.    _._./ ;
                                                | |                 .__ )-'\  __  \  \  I   1   / /
                                                `-'                /   `-\-(-'   \ \  `.|   | ,' /
                                                                   \__  `-'    __/  `-. `---'',-'
                                                                      )-._.-- (        `-----'
                                                                     )(  l\ o ('..-.
                                                               _..--' _'-' '--'.-. |
                                                        __,,-'' _,,-''            \ \
                                                       f'. _,,-'                   \ \
                                                      ()--  |                       \ \
                                                        \.  |                       /  \
                                                          \ \                      |._  |
                                                           \ \                     |  ()|
                                                            \ \                     \  /
                                                             ) `-.                   | |
                                                            // .__)                  | |
                                                         _.//7'                      | |
                                                       '---'                         j_| `
                                                                                    (| |
                                                                                     |  \
                                                                                     |lllj
                                                                                     ||||| ");
                Console.WriteLine("\n СКЕЛЕТ ИДЕТ НА ВАС !!!");
                Console.ResetColor();


                break;

            case 2: // ОГР

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("\n Вы победили скелета волшебным скипетером !");
                Console.WriteLine(@"
                                                              ....
                                                            .'' .'''
                            .                             .'   :
                            \\                          .:    :
                             \\                        _:    :       ..----.._
                              \\                    .:::.....:::.. .'         ''.
                               \\                 .'  #-. .-######'     #        '.
                                \\                 '.##'/ ' ################       :
                                 \\                  #####################         :
                                  \\               ..##.-.#### .''''###'.._        :
                                   \\             :--:########:            '.    .' :
                                    \\..__...--.. :--:#######.'   '.         '.     :
                                    :     :  : : '':'-:'':'::        .         '.  .'
                                    '---'''..: :    ':    '..'''.      '.        :'
                                       \\  :: : :     '      ''''''.     '.      .:
                                        \\ ::  : :     '            '.      '      :
                                         \\::   : :           ....' ..:       '     '.
                                          \\::  : :    .....####\\ .~~.:.             :
                                           \\':.:.:.:'#########.===. ~ |.'-.   . '''.. :
                                            \\    .'  ########## \ \ _.' '. '-.       '''.
                                            :\\  :     ########   \ \      '.  '-.        :
                                           :  \\'    '   #### :    \ \      :.    '-.      :
                                          :  .'\\   :'  :     :     \ \       :      '-.    :
                                         : .'  .\\  '  :      :     :\ \       :        '.   :
                                         ::   :  \\'  :.      :     : \ \      :          '. :
                                         ::. :    \\  : :      :    ;  \ \     :           '.:
                                          : ':    '\\ :  :     :     :  \:\     :        ..'
                                             :    ' \\ :        :     ;  \|      :   .'''
                                             '.   '  \\:                         :.''
                                              .:..... \\:       :            ..''
                                             '._____|'.\\......'''''''.:..'''
                                                        \\
                            ");
                Console.ResetColor();
                Continue();



                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(@"


                                                                                  , --,  
                                           ,                   \,       '-,-`,'-.' | ._,.-.
                                          /|           \    ,   |\         }  )/  / `-,',
                                          [ '          |\  /|   | |        /  \|  |/`  ,`
                                          | |       ,.`  `,` `, | |  _,...(   (      _',
                                          \  \  __ ,-` `  ,  , `/ |,'      Y     (   \_L\
                                           \  \_\,``,   ` , ,  /  |         )         _,/
                                            \  '  `  ,_ _`_,-,<._.<        /         /
                                             ', `>.,`  `  `   ,., |_      |         /
                                               \/`  `,   `   ,`  | /__,.-`    _,   `\
                                           -,-..\  _  \  `  /  ,  / `._) _,-\`       \
                                            \_,,.) /\    ` /  / ) (-,, ``    ,        |
                                           ,` )  | \_\       '-`  |  `(               \
                                          /  /```(   , --, ,' \   |`<`    ,            |
                                         /  /_,--`\   <\  V /> ,` )<_/)  | \      _____)
                                   ,-, ,`   `   (_,\ \    |   /) / __/  /   `----`
                                  (-, \           ) \ ('_.-._)/ /,`    /
                                  | /  `          `/ \\ V   V, /`     /
                               ,--\(        ,     <_/`\\     ||      /
                              (   ,``-     \/|         \-A.A-`|     /
                             ,>,_ )_,..(    )\          -,,_-`  _--`
                            (_ \|`   _,/_  /  \_            ,--`
                             \( `   <.,../`     `-.._   _,-`
                              `                      ```

");
                Console.WriteLine("\n ПЕРЕД ВАМИ ВСТАЕТ ГИГАНТСКИЙ ОГР !");
                Console.ResetColor();
                break;

            case 3: // ДРАКОН
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(@"
                                        ))))))))`""###mnn$$$$""""""%%%%%%%`""""$$$$$$$mmn..m###(((((((((((((((((((((((((((((
                                        ))))))))))""$$$""""'%%%%%%%%%%%%%%%%%%%%%%`""""$$$$$n###.(((((((((((((((((((((((((.
                                        )))))))))))$##%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%`""$$$n#.(((((((((((((((((((.m###
                                        )))))))))))`##n%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%`""$$$n.((((((((((((((.m####""'
                                        ))))))))))))###%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%`""$$n.(((((((((.m####""'((
                                        ))))))))))))""##m%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%`""$$n((((((.m###""'(((((
                                        mmmmn..))))))###%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%`$$n((.m###""'(((((((
                                        ###########n.###%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%`$$n(###""'(((((((((
                                        ))))`""""""########%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%""$$.#""'((((((((((
                                        )))))))))))`m##""%%%%%%%m$$%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%`$$""((((((((((((
                                        ))))))))))))m##%%%%%%%m$$'%%%%%%%%%%%%%%%%%%%%%$n.%%%%%%%%%%$$'(((((((((((((
                                        ))))))))))))##""%%%%%%m$$""%%%%%%%%%%%%%%%%.n%%%%""$$n.%%%%%$$.$""((((((((((((((
                                        )))))))))))%##%%%%%%%$$n%%%%%%%%%%%%%%%%m$$n%%%%`""$$n.%%%`$$n.((((((((((((((
                                        ))))))))))m##""%%%%%%%$$$%%%%%%%%%%%%%%%m$$$$n.%%%%`""$$n.%%`""$$n(((((((((((((
                                        ))))))).m##""%%%%%%%%%$$$%%%%%%%%%%%%%%%$$""'""$$n.%%%%`""$$.%%%`$$n.(((((((((((
                                        ))))).m##""%%%%%%%%%%%""$$n%%%%%%%%%%%%%%$$n `""$$n%%%%%`$$.%%%`""$$$((((((((((
                                        )))))m##""'%%%%%%%%%%%%$$$%%%%%%%%%%%%%%`$$n `""$$n%%%%`$$%%%%%`$""##n(((((((
                                        ))).m##""%%%%%%%%%%%%%m""$$n%%%%%%%%%%%%%%`""$$n ""$$.%%%%$$%%%%m$"" ""##n((((((
                                        ).m##""'%%%%%%%%%%%%%m$$""$$%%%%%%%%%%%%%%%%""$$n. ""$$%%%%`$n%.m$"" ""##n(((((
                                        m##""'%%%%%%%%%%%%%%%$$':$$n%%%%%%%%%%%%%%%%`""$$n. $$%%%%%""$n$$"" ""##n(((((
                                        ##""%%%%%%%%%%%%%%%%m$$::""$$n%%%%%%%%%%%%%%%%%`""$$$$""%%%%%%$$.$ ""##(((((
                                        #""%%%%%%%%%%%%%%%%%$$':::""$$n.%%%%%%%%%%%%%%%%%%`$'%%%%%%%$$n"" ##n(((m
                                        %%%%%%%%%%%%%%%%%%%$$::::::""$$m.%%%%%%%%%%%%%%%%%%%%%%%%%%`""$$. `##n(.m#
                                        %%%%%%%%%%%%%%%%%%%$$::::::::`""$$n.%%%%%%%%%%%%%%%%%%%%%%%%%`""$n. `##m##""
                                        %%%%%%%%%%%%%%%%%%%$$n:::::::::$`$$n.%%%%%%%%%%%%%%%%%%%%%%%%%""$$. m##""'
                                        %%%%%%%%%%%%%%%%%%%""$$:::::::::`$$""$$n%%%%%%%%%%%%%%%%%%%%%%%%%""$n nW'""
                                        %%%%%%%%%%%%%%%%%%%%$$::::::::::$$ `$$n%%%%%%%%%%%%%%%%%%%%%%%%""$$. ''
                                        %%%%%%%%%%%%%%%%%%%%$$n:::::::::$$n ""$$%%%%%%%%%%%%%%%%%%%%%%%%`$$n.
                                        %%%%%%%%%%%%%%%%%%%%""$$:::::::::""$$ $$n%%%%%%%%%%%%%%%%%%%%%%%""$$n
                                        %%%%%%%%%%%%%%%%%%%%%$$::::::::::$$ `$$n%%%%%%%%%%%%%%%%%%%%%%""$$
                                        %%%%%%%%%%%%%%%%%%%%%$$::::::::::$$ ""$$n.m$$$n%%%%%%%%%%%%%%%%$$n
                                        %%%%%%%%%%%%%%%%%%%%%$$n:::::::::$$n .m$$$""""""$n%%%%%%%%%%%%%%%`$$n
                                        %%%%%%%%%%%%%%%%%%%%%`$$:::::::::""$$ $$$""'%%%`""%%%%%%%%%%%%%%%%n$""
                                        %%%%%%%%%%%%%%%%%%%%%%$$::::::::::$$ $$$n%%%%%%%%%%%%%%%%%%%%%.m$$n.
                                        %%%%%%%%%%%%%%%%%%%%%%$$::::::::::$$n `$$$.%%%%%%%%%%%%%%%%%%%%""$$n""$
                                        %%%%%%%%%%%%%%%%%%%%%%$$n:::::::::""$$ `""$$.%%%%%%$$n$m.%%%%%%%%""$$n'$.
                                        %%%%%%%%%%%%%%%%%%%%%%`$$::::::::::$$n `$$n.%%%%""$$""""$n%%%%%%%%`""$n`$n.
                                        %%%%%%%%%%%%%%%%%%%%%%%$$::::::::::`$$ `$$n.%%%`$nm$""%%%%%%%%%%""$$`$
                                        %%%%%%%%%%%%%%%%%%%%%%%$$:::::::::::$$n `""$$n%%`""$$n%%%%%%%%%%%$$n$'
                                        %%%%%%%%%%%%%%%%%%%%%%%$$n::::::::::`$$ `""$mn.`""$n.%%%%%%%%%""$$'
                                        %%%%%%%%%%%%%%%%%%%%%%%""$$:::::::::::$$n `""$$n.`""$$.%%%%%%%.$$
                                        %%%%%%%%%%%%%%%%%%%%%%%%$$:::::::::::`$$ `$$n.`$$.%%%%%.$$""
                                        %%%%%%%%%%%%%%%%%%%%%%%%$$n:::::::::::$$n `""$$n`$$.%%%%.$$
                                        %%%%%%%%%%%%%%%%%%%%%%%%""$$:::::::::::`$$ `""$n.""$.%%m$$
                                        %%%%%%%%%%%%%%%%%%%%%%%%%$$::::::::::::$$n `""$n$$.$$$'
                                        %%%%%%%%%%%%%%%%%%%%%%%%%$$::::::::::::`$$ ""$$$$$""'
                                        %%%%%%%%%%%%%%%%%%%%%%%%%$$n::::::::::::$$n .$$W""'
                                        %%%%%%%%%%%%%%%%%%%%%%%%%`$$::::::::::::`$$ .$W""");
                Console.WriteLine("\n ДРАКОН ИЗДАЕТ ОГЛУШИТЕЛЬНЫЙ РЕВ!");
                Console.ResetColor();
                break;

            case 4: // паук

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nЗа поворотом, в густой тьме, Вы пробираетесь через завесу из бледных, липких прядей. Они цепляются за доспехи, словно пальцы умерших. . .");
                Console.WriteLine(@" 
                                                    ░░░░░░░░░░░░█░░░░░░░░░░░░░░█░░░░░░░░░░░░
                                                    ░░░░░░░░░░░░█▄░░░░░░░░░░░░▄█░░░░░░░░░░░░
                                                    ░░░░░░░░░░░░░█▄▄░░░░░░░░░▄█░░░░░░░░░░░░░
                                                    ░░░░░░░░░░░░░███▀█▄▄▄▄█▀███░░░░░░░░░░░░░
                                                    ░░░░░░░░░░░░█░▀█░░░░░░░░█░░█░░░░░░░░░░░░
                                                    ░░░░░░░░░░▄█▀░░██▄▄▄▄▄▄██░░▀█▄░░░░░░░░░░
                                                    ▀▀▀█▄▄▄▄▀▀▀░░░▄█▀█░░░░█▀█▄░░░▀▀▀▄▄▄▄█▀▀▀
                                                    ░░░░░▀███▄▄▄▄█▀░░█▄░░▄█░░▀█▄▄▄▄████░░░░░
                                                    ░░░░░░█▄░░▀██▄▄▄░░█▄▄█░░▄▄▄██▀░░▄█░░░░░░
                                                    ░░░░░░░█░░░█░░░▀▀██████▀▀░░░█░░░█░░░░░░░
                                                    ░░░░░░░█░░░█░░░▄▄██████▄▄░░░█░░░█░░░░░░░
                                                    ░░░░░░█▀░░▄██▀▀▀░░█▀▀█░░▀▀▀██▄░░▀█░░░░░░
                                                    ░░░░░███▀▀▀▀▀█▄░░█▀░░▀█░░▄█▀▀▀▀███▄░░░░░
                                                    ▄▄▄█▀▀▀▀▄▄▄░░░▀█▄█░░░░█▄█▀░░░▄▄▄▀▀▀▀█▄▄▄
                                                    ░░░░░░░░░░▀█▄░░██▀▀▀▀▀▀██░░▄█▀░░░░░░░░░░
                                                    ░░░░░░░░░░░░█░▄█░░░░░░░░█▄░█░░░░░░░░░░░░
                                                    ░░░░░░░░░░░░░███▄█▀▀▀▀█▄███░░░░░░░░░░░░░
                                                    ░░░░░░░░░░░░░█▀░░░░░░░░░▀▀█░░░░░░░░░░░░░
                                                    ░░░░░░░░░░░░█▀░░░░░░░░░░░░▀█░░░░░░░░░░░░
                                                    ░░░░░░░░░░░░█░░░░░░░░░░░░░░█░░░░░░░░░░░░
                ");
                Console.WriteLine("И тогда вы понимаете: паутина — не просто преграда. Она плетет гигантский лик, взирающий на вас пустыми глазницами. . .");
                Console.WriteLine("Это знак, который не спутать ни с чем: сама Смерть взирает на вас из паутины. . .");
                Console.ResetColor();
                Continue();







                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(@"
                            
           ;               ,                
         ,;                 '.         
        ;:                   :;        
       ::                     ::       
       ::                     ::       
       ':                     :        
        :.                    :        
     ;' ::                   ::  '     
    .'  ';                   ;'  '.    
   ::    :;                 ;:    ::   
   ;      :;.             ,;:     ::   
   :;      :;:           ,;""      ::   
   ::.      ':;  ..,.;  ;:'     ,.;:   
    ""'...   '::,::::: ;:   .;.;""'    
        '""""....;:::::;,;.;"""".-.
    .:::'.....'"":::::::'""....;::::;.
   ;:' '""'"";.,;:::::;.'""""""'  ':;
  ::'         ;::;:::;::..         :;
 ::         ,;:::::::::::;:..       ::
 ;'     ,;;:;::::::::::::::;"";..    ':
::     ;:""  ::::::""""'::::::  "":     ::
 :.    ::   ::::::;  :::::::   :     ;
  ;    ::   :::::::  :::::::   :    ;
   '   ::   ::::::....:::::'  ,:   '
    '  ::    :::::::::::::""   ::
       ::     ':::::::::""'    ::
       ':       """"""""'      ::
        ::                   ;:
        ':;                 ;:""
-Tss-     ';              ,;'
            ""'           '""
              '

                    ");
                Console.WriteLine("\n ГИГАНТСКИЙ ПАУК ПОЛЗЕТ НА ВАС!!!");
                Console.ResetColor();
                break;
        }
    }

    static void GameOver(Player player)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(@"
                   __     ______  _    _   _____ _____ ______ _____  
                 \ \   / / __ \| |  | | |  __ \_   _|  ____|  __ \ 
                  \ \_/ / |  | | |  | | | |  | || | | |__  | |  | |
                   \   /| |  | | |  | | | |  | || | |  __| | |  | |
                    | | | |__| | |__| | | |__| || |_| |____| |__| |
                    |_|  \____/ \____/  |_____/_____|______|_____/ 
                                                                   ");
        Console.WriteLine($"\n {player.Name} пал в бою. . .");
        Console.WriteLine($"  Финальный счет: {player.Score}");
        Console.ResetColor();

        Console.WriteLine("\nИгра завершится через 5 секунд. . .");
        Thread.Sleep(5000);
    }

    static void Victory(Player player)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(@"


                                    _
                                   (_)
                                   |=|
                                   |=|
                               /|__|_|__|\
                              (    ( )    )
                               \|\/\""/\/|/
                                 |  Y  |
                                 |  |  |
                                 |  |  |
                                _|  |  |
                             __/ |  |  |\
                            /  \ |  |  |  \
                               __|  |  |   |
                            /\/  |  |  |   |\
                             <   +\ |  |\ />  \
                              >   + \  | LJ    |
                                    + \|+  \  < \
                              (O)      +    |    )
                               |             \  /\ 
                             ( | )   (o)      \/  )
                            _\\|//__( | )______)_/ 
                                    \\|//        
                            ");
        Console.WriteLine("\n  ВЫ ПРОШЛИ ВСЕ КАТАКОМБЫ!");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(@"
                                         o                o                                         
                                     _<|>_             <|>                                        
                                                       < >                                        
                          o      o     o        __o__   |        o__ __o    \o__ __o    o      o  
                         <|>    <|>   <|>      />  \    o__/_   /v     v\    |     |>  <|>    <|> 
                         < >    < >   / \    o/         |      />       <\  / \   < >  < >    < > 
                          \o    o/    \o/   <|          |      \         /  \o/         \o    o/  
                           v\  /v      |     \\         o       o       o    |           v\  /v   
                            <\/>      / \     _\o__</   <\__    <\__ __/>   / \           <\/>    
                                                                                           /      
                                                                                          o       
                                                                                       __/>       ");
        Console.WriteLine($"\n  {player.Name} СТАЛ ЛЕГЕНДОЙ КАТАКОМБ!");
        Console.WriteLine($" ФИНАЛЬНЫЙ СЧЕТ: {player.Score} очков!");
        Console.ResetColor();

        Console.WriteLine("\nИгра завершится через 15 секунд...");
        Thread.Sleep(15000);
    }

    static void Continue()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\nНажмите любую клавишу чтобы продолжить...");
        Console.ResetColor();
        Console.ReadKey();
    }
}