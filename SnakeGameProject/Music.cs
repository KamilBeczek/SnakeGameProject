using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SnakeGameProject
{
    public class Music
    {

        public readonly static SoundPlayer EatSound = new SoundPlayer("Assets/eat.wav");
        public readonly static SoundPlayer DeadSound = new SoundPlayer("Assets/dead.wav");


        public void LoadMusic(SoundPlayer music)
        {
            music.Load();
        }

        public void PlayMusic(SoundPlayer music)
        {
            music.Play();
        }




    }
}
