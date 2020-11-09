using System;
using System.Threading.Tasks;
using Xamanimation;
using AppName.Core;

namespace AppName
{
    public class TranslateXAnimation : TranslateToAnimation, ITriggerAction
    {
        public Task Execute()
        {
            return BeginAnimation();
        }

        protected override Task BeginAnimation()
        {
            TranslateY = Target.TranslationY;

            return base.BeginAnimation();
        }
    }
}
