﻿using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR;

namespace Exprazor.AspNetCore.Sandbox
{
    public class ExprazorRouter
    {
        IEndpointRouteBuilder builder;
        public ExprazorRouter(IEndpointRouteBuilder builder)
        {
            this.builder = builder;
        }

        record RoutingUnit(Regex Pattern, Func<string[]?, ExprazorApp> Initializer);

        List<RoutingUnit> _routes = new();

        public ExprazorApp? Get(string path)
        {
            foreach (var unit in _routes)
            {
                if (unit.Pattern.IsMatch(path))
                {
                    var matches = unit.Pattern.Matches(path);
                    return unit.Initializer(matches.Select(x => x.Value).ToArray());
                }
            }

            return null;
        }
        public void Route(string pattern, Func<string[]?, ExprazorApp> initializer)
        {
            var regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
            _routes.Add(new RoutingUnit(regex, initializer));
        }
    }
}
