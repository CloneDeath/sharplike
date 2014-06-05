using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Sharplike.Core.Audio;
using OpenTK.Audio;

namespace Sharplike.Audio.TK
{
    public class OpenTKAudioEngine : AbstractAudioEngine
    {
        public OpenTKAudioEngine()
        {
            ac = new AudioContext();
            ac.CheckErrors();
        }
        public override AbstractAudioCue BuildAudioCue(Stream audioData)
        {
            return new OpenTKAudioCue(audioData, ac);
        }

        public override void Process()
        {
            ac.Process();
            ac.CheckErrors();
        }

        public override void Dispose()
        {
            ac.Dispose();
        }

        AudioContext ac;
    }
}
