//
// C# version of the code presented in the reference below.
// Converion by: Bob Elward Feb 2021
//
/*
 * Copyright (c) 2005 Brian Goetz and Tim Peierls
 * Released under the Creative Commons Attribution License
 *   (http://creativecommons.org/licenses/by/2.5)
 * Official home: http://www.jcip.net
 *
 * Any republication or derived work distributed in source code form
 * must include this copyright and license notice.
 */
//
using System;

namespace JcipAnnotations
{
    //   
    //   The class to which this annotation is applied is immutable.  This means that
    //   its state cannot be seen to change by callers, which implies that
    //   
    //     - all public fields are final,
    //     - all public final reference fields refer to other immutable objects, and
    //     - constructors and methods do not publish references to any internal state
    //       which is potentially mutable by the implementation.
    //   
    //   Immutable objects may still have internal mutable state for purposes of performance
    //   optimization; some state variables may be lazily computed, so long as they are computed
    //   from immutable state and that callers cannot tell the difference.
    //   
    //   Immutable objects are inherently thread-safe; they may be passed between threads or
    //   published without synchronization.
    //   
    [AttributeUsage(AttributeTargets.Class
                  , AllowMultiple = true)]

    public class Immutable : Attribute
    {
    }
}
