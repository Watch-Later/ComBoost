﻿using DotNetCore.CAP.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wodsoft.ComBoost.Distributed.CAP
{
    public class DomainConsumerServiceSelector : IConsumerServiceSelector
    {
        private DomainCAPEventProvider _provider;

        public DomainConsumerServiceSelector(DomainCAPEventProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public ConsumerExecutorDescriptor SelectBestCandidate(string key, IReadOnlyList<ConsumerExecutorDescriptor> candidates)
        {
            if (candidates.Count == 0)
                return null;
            return candidates.FirstOrDefault(x => x.TopicName.Equals(key, StringComparison.InvariantCultureIgnoreCase));
        }

        public IReadOnlyList<ConsumerExecutorDescriptor> SelectCandidates()
        {
            return _provider.Handlers.Select(t =>
            {
                var argType = t.Method.GetParameters()[1].ParameterType;
                var descriptor = (DomainConsumerExecutorDescriptor)Activator.CreateInstance(typeof(DomainConsumerExecutorDescriptor<>).MakeGenericType(argType), t);
                return descriptor;
            }).ToList();
        }
    }
}
