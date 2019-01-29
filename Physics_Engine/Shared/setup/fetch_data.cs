using System;
using System.Windows.Input;
using System.IO;

namespace Application
{
    public struct g_values
    {
        String Name;
        String ID;
        bool Solid;
        String Type;
        int defaultZ;
    }

    public struct e_values
    {
        String ID;
        int Speed;
        int Health;
        int Strength;
    }

    public struct b_values
    {
        String ID;
        bool Climbable;
        int height;
        bool Traversable;
    }

    public class fetch
    {
        string location = Directory.GetCurrentDirectory();
        string path_g = Directory.GetParent(location)+"/Resources/tables/";
        public fetchData(String ID)
        {
            path_g 
            return struct data;
        }
    }
}
