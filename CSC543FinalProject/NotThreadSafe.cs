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
    //   The class to which this annotation is applied is not thread-safe.
    //   This annotation primarily exists for clarifying the non-thread-safety of a class
    //   that might otherwise be assumed to be thread-safe, despite the fact that it is a bad
    //   idea to assume a class is thread-safe without good reason.
    //   @see ThreadSafe
    //   
    [AttributeUsage(AttributeTargets.Class
                  , AllowMultiple = true)]

    public class NotThreadSafe : Attribute
    {
    }
}
