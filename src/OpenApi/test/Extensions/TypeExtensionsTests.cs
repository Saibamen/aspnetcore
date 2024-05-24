// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;

public class TypeExtensionsTests
{
    private delegate void TestDelegate(int x, int y);

    private class Container
    {
        internal delegate void ContainedTestDelegate(int x, int y);
    }

    private class Outer<T>
    {
        public class Inner
        {
        }
        public class Inner2<U>
        {
        }
    }

    private class Outer
    {
        public class Inner<T>
        {
        }
    }

    public static IEnumerable<object[]> GetSchemaReferenceId_Data =>
    [
        [typeof(Todo), "Todo"],
        [typeof(IEnumerable<Todo>), "IEnumerableOfTodo"],
        [typeof(TodoWithDueDate), "TodoWithDueDate"],
        [typeof(IEnumerable<TodoWithDueDate>), "IEnumerableOfTodoWithDueDate"],
        [(new { Id = 1 }).GetType(), "AnonymousTypeOfInt32"],
        [(new { Id = 1, Name = "Todo" }).GetType(), "AnonymousTypeOfInt32AndOfString"],
        [typeof(IFormFile), "IFormFile"],
        [typeof(IFormFileCollection), "IFormFileCollection"],
        [typeof(Results<Ok<TodoWithDueDate>, Ok<Todo>>), "ResultsOfOkOfTodoWithDueDateAndOfOkOfTodo"],
        [typeof(Ok<Todo>), "OkOfTodo"],
        [typeof(NotFound<TodoWithDueDate>), "NotFoundOfTodoWithDueDate"],
        [typeof(TestDelegate), "TestDelegate"],
        [typeof(Container.ContainedTestDelegate), "ContainedTestDelegate"],
        [(new int[2, 3]).GetType(), "IntArrayOf2"],
        [typeof(List<int>), "ListOfInt32"],
        [typeof(List<>), "ListOfT"],
        [typeof(List<List<int>>), "ListOfListOfInt32"],
        [typeof(int[]), "IntArray"],
        [typeof(int[,]), "IntArrayOf2"],
        [typeof(Outer<>.Inner), "InnerOfT"],
        [typeof(Outer<int>.Inner), "InnerOfInt32"],
        [typeof(Outer<>.Inner2<>), "InnerOfTAndOfU"],
        [typeof(Outer<int>.Inner2<int>), "InnerOfInt32AndOfInt32"],
        [typeof(Outer.Inner<>), "InnerOfT"],
        [typeof(Outer.Inner<int>), "InnerOfInt32"],
    ];

    [Theory]
    [MemberData(nameof(GetSchemaReferenceId_Data))]
    public void GetSchemaReferenceId_Works(Type type, string referenceId)
        => Assert.Equal(referenceId, type.GetSchemaReferenceId());
}
