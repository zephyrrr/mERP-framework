using System;
using System.Collections.Generic;
using System.Text;
using System.Media;
using System.IO;

namespace Feng.Windows.Utils
{
    /// <summary>
    /// …˘“Ùµƒ∞Ô÷˙¿‡
    /// </summary>
    public static class SoundHelper
    {
        private static SoundPlayer s_player = new SoundPlayer();

        /// <summary>
        /// ≤•∑≈…˘“Ù
        /// </summary>
        /// <param name="fileName"></param>
        public static void PlaySound(string fileName)
        {
            try
            {
                s_player.SoundLocation = fileName;
                //m_player.LoadAsync();
                s_player.PlaySync();
            }
            catch (Exception ex)
            {
                 ExceptionProcess.ProcessWithResume(ex);
            }
        }

        /// <summary>
        /// ≤•∑≈…˘“Ù
        /// </summary>
        /// <param name="stream"></param>
        public static void PlaySound(Stream stream)
        {
            try
            {
                s_player.Stream = stream;
                s_player.PlaySync();
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
            }
        }
    }
}