using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToyBox
{
    public class ActiveAnimationSetUpdater
    {
        private Set<Animation> animations;

        public ActiveAnimationSetUpdater(Set<Animation> animations)
        {
            this.animations = animations;
        }

        public void Add(Animation animation)
        {
            animation.Started += new EventHandler<EventArgs>(Animation_Started);
            animation.Finished += new EventHandler<EventArgs>(Animation_Finished);
        }

        private void Animation_Started(object sender, EventArgs args)
        {
            animations.Add((Animation)sender);
        }

        private void Animation_Finished(object sender, EventArgs args)
        {
            animations.Remove((Animation)sender);
        }
    }
}
