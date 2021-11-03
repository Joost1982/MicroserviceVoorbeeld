using System;

namespace LogExampleLibrary
{
    // voorliggende klasse (en hele library) als test toegevoegd om uit te proberen hoe je een library kan referencen
    public static class Logger
    {

        public static void Log(string bericht)
        {
            Console.WriteLine("--> Dit is een prachtig logbericht gegereneerd met functionaliteit vanuit de LogExampleLibrary.");
            Console.WriteLine($"--> Bericht: {bericht}");
            Console.WriteLine("--> Einde bericht.");
        }
    }
}
