﻿using System;
using Autofac;
using TagsCloud.CloudRenderers;
using TagsCloud.ContainerConfigurators;
using TagsCloud.StatisticProviders;
using TagsCloud.WordLayouters;
using TagsCloud.WordReaders;
using IContainer = Autofac.IContainer;

namespace TagsCloud
{
    public static class Program
    {
        public static void Main()
        {
            while (true)
            {
                MakeCloud();
            }
        }

        public static void MakeCloud(IContainer container = null)
        {
            container ??= new ConsoleContainerConfigurator().Configure();

            Result.Of(() => container.Resolve<IWordReader>().ReadWords())
                .Then(words => container.Resolve<IStatisticProvider>().GetWordStatistics(words))
                .Then(statistic => container.Resolve<IWordLayouter>().AddWords(statistic))
                .Then(none => container.Resolve<ICloudRenderer>().RenderCloud())
                .Then(path => Console.WriteLine($"Cloud saved in {path}"))
                .OnFail(Console.WriteLine);
        }
    }
}