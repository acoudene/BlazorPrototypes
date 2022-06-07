using CascadingBlazorApp.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();











// From examples in: https://chrissainty.com/understanding-cascading-values-and-cascading-parameters/

//Cascading values and parameters are a way to pass a value from a component to all of its descendants without having to use traditional component parameters.

//Blazor comes with a special component called CascadingValue. This component allows whatever value is passed to it to be cascaded down its component tree to all of its descendants. The descendant components can then choose to collect the value by declaring a property of the same type, decorated with the [CascadingParameter] attribute.

//Basic Usage
//You can setup a cascading value as follows.

//HTML<CascadingValue Value="@TheAnswer">
//    <FooComponent></FooComponent>
//</CascadingValue>

//@code {
//    int TheAnswer = 42;
//}
//As I said previously, FooComponent can make use of the value being cascaded down by declaring a property of the same type, decorated with the [CascadingParameter] attribute.

//HTML<h1> Foo Component</h1>

//<p>The meaning of life is @MeaningOfLife.</p>

//@code {
//    [CascadingParameter] int MeaningOfLife { get; set; }
//}
//The MeaningOfLife property will automatically be populated with 42 when the component is rendered in the same way as a standard component [Parameter] property.

//Just like component parameters, if a cascading value is changed the change will be passed down to all descendants. And any components using the value will be updated and automatically have StateHasChanged called.  

//Multiple Cascading Parameters
//You may have noticed in the example above that there wasn't any way of identifying the cascading value. The FooComponent just declares a property as a [CascadingParameter] and the value gets set. Which is fine when there is only one cascading parameter. But what happens when you have two, or three, or more?

//There needs to be a way of identifying which one is which. As it happens, there are in-fact two ways of identifying cascading parameters.

//By Type
//The first is provided by the framework and is based on types. Say we had two cascading values, one is a string and one is an int. And there is a single child component.

//HTML<CascadingValue Value="@FruitName">
//    <CascadingValue Value="@FruitCount">
//        <Fruit></Fruit>
//    </CascadingValue>
//</CascadingValue>

//@code {
//    string FruitName { get; set; } = "Apple";
//int FruitCount { get; set; } = 111;
//}
//The Fruit component declares a cascading parameter as follows.

//HTML<p>The fruit is: @Name </ p >

//@code {
//  [CascadingParameter] string Name { get; set; }
//}
//Blazor will look at the type of the Name parameter and try and find a cascading value which matches. In this case, it will match FruitName and bind Name to its value.

//You may be thinking, that's great, but what happens if both cascading values have the same type?

//In that situation, the framework is still going to match based on type. Except it will use the closest ancestor to the component requesting the parameter. Modifying the previous example just a bit, we can see how this works.

//HTML<CascadingValue Value="@FirstFruit">
//    <CascadingValue Value="@SecondFruit">
//        <Fruit></Fruit>
//    </CascadingValue>
//</CascadingValue>

//@code {
//    string FirstFruit { get; set; } = "Apple";
//string SecondFruit { get; set; } = "Banana";
//}
//The Fruit component is the same as it was before.

//HTML<p>The fruit is: @Name </ p >

//@code {
//  [CascadingParameter] string Name { get; set; }
//}
//This time round both cascading values are of type string. The framework is going to look for the closest ancestor to the Fruit component with a matching type. In this scenario the matching value will be SecondFruit.

//By Name
//The second, and most reliable way to identify cascading parameters is by name. When you create a cascading value you have the option to give it a name. Then when a child component wants to use it, they can ask for it by specifically.

//Going back to our previous example with the two fruits, if we name the two cascading values.

//HTML<CascadingValue Value="@FirstFruit" Name="FirstFruit">
//    <CascadingValue Value="@SecondFruit" Name="SecondFruit">
//        <Fruit></Fruit>
//    </CascadingValue>
//</CascadingValue>

//@code {
//    string FirstFruit { get; set; } = "Apple";
//string SecondFruit { get; set; } = "Banana";
//}
//The Fruit component can now be specific about which value it wants to use.

//HTML<p>The fruit is: @Name </ p >

//@code {
//  [CascadingParameter(Name = "FirstFruit")] string Name { get; set; }
//}
//Performance
//This may all sound good but, what about performance?

//All these cascading values are active by default. What do I mean by active? Well, if a cascading value is changed then the new value will be sent down the component tree and all components that use it will be updated. Therefor, Blazor has to keep a watch on the value continuously. This takes up resource and in a large application could end up causing performance issues.

//But what if you know your cascading value will never change? It would be nice to be able to tell Blazor to not have to keep a watch on it and not take up that resource. Well, you can.

//On the CascadingValue component there is a IsFixed parameter. It is set to false by default but if you set it too true you are telling Blazor to not monitor it for changes.

//HTML<CascadingValue Value="@Fruit" IsFixed="true">
//    <Fruit></Fruit>
//</CascadingValue>

//@code {
//    string Fruit { get; set; } = "Peach";
//}
//Now Fruit is a fixed value and the framework won't use up any resources setting up change detection.

//Updating Cascading Values
//There has been a bit of confusion when it comes to updating cascading values. The important thing to understand is that updates only cascade down you can't update a value from a descendant.

//For example, say we had two components FruitBowl and LunchBox, which both received a cascading value.

//HTML<CascadingValue Value="@Fruit">
//    <FruitBowl></FruitBowl>
//    <LunchBox></LunchBox>
//</CascadingValue>

//@code {
//    string Fruit { get; set; } = "Kiwi";
//}
//HTML < !--FruitBowl Component-- >

//< p > Fruit bowl contains @FruitName</p>

//<button @onclick="ChangeFruit">Change Fruit</button>

//@code {
//    [CascadingParameter] string FruitName { get; set; }

//private void ChangeFruit()
//{
//  FruitName = "Pineapple";
//}
//}
//HTML < !--LunchBox Component-- >

//< p > Lunch box contains @FruitName</p>

//@code {
//    [CascadingParameter] string FruitName { get; set; }
//}
//If we run this code the output, ignoring the button, would look like this.

//Fruit bowl contains Kiwi
//Lunch box contains Kiwi

//If we click the Change Fruit button in the FruitBowl component, it will not trigger an update in the LunchBox component. The output would look like this.

//Fruit bowl contains Pineapple
//Lunch box contains Kiwi

//If you need to update a cascading value from a descendant then you will need to choose a different mechanism to achieve it. I've got a couple of options to show you, the first is using events.

//Using Events
//Using the example above, we can modify it to use an event to trigger an update of the cascading value.

//HTML<CascadingValue Value="@Fruit">
//    <FruitBowl OnFruitChange="ChangeFruit"></FruitBowl>
//    <LunchBox></LunchBox>
//</CascadingValue>

//@code {
//    private string Fruit { get; set; } = "Kiwi";

//private void ChangeFruit(string newFruit)
//{
//  Fruit = newFruit;
//  StateHasChanged();
//}
//}
//HTML < !--FruitBowl Component-- >

//< p > Bowl contains @Fruit</p>

//<button @onclick = "ChangeFruit" > Change Fruit</button>

//@code {

//    [CascadingParameter] string Fruit { get; set; }

//    [Parameter] public Action<string> OnFruitChange { get; set; }

//private void ChangeFruit()
//{
//  OnFruitChange?.Invoke("Pineapple");
//}

//}
//HTML < !--LunchBox Component-- >

//< p > Lunch box contains @Fruit</p>

//@code {
//    [CascadingParameter] string Fruit { get; set; }
//}
//Now when we run the code above we will get the same initial output as before.

//Fruit bowl contains Kiwi
//Lunch box contains Kiwi

//But now when we click the Change Fruit button we will get the following.

//Fruit bowl contains Pineapple
//Lunch box contains Pineapple

//Using Complex Types
//Another option is to pass a complex type down instead of an individual property, a component instance for example. Descendant components can then perform actions against the instance using its methods and bind to its properties.

//Let's look at an example.

//HTML<!-- FruitDispenser Component -->

//<CascadingValue Value="this">
//    <FruitBowl></FruitBowl>
//    <LunchBox></LunchBox>
//</CascadingValue>

//@code {
//    public string Fruit { get; private set; } = "Kiwi";

//public void ChangeFruit(string newFruit)
//{
//  Fruit = newFruit;
//  StateHasChanged();
//}
//}
//HTML < !--FruitBowl Component-- >

//< p > Bowl contains @FruitDispenser.Fruit</p>

//<button @onclick = "ChangeFruit" > Change Fruit</button>

//@code {

//  [CascadingParameter] FruitDispenser FruitDispenser { get; set; }

//  [Parameter] public Action<string> OnFruitChange { get; set; }

//  private void ChangeFruit()
//  {
//    FruitDispenser.ChangeFruit("Pineapple");
//  }

//}
//HTML < !--LunchBox Component-- >

//< p > Lunch box contains @FruitDispenser.Fruit</p>

//@code {
//    [CascadingParameter] FruitDispenser FruitDispenser { get; set; }
//}
//Just as in the previous example using events, when we run the code above we will get this initial output.

//Fruit bowl contains Kiwi
//Lunch box contains Kiwi

//And when we click the Change Fruit button we will continue to get the following.

//Fruit bowl contains Pineapple
//Lunch box contains Pineapple

//As you can see we have achieved the same result as using events and with a bit less code.

//The question is, should we really be passing complex types around like this just to update a single property value?

//Drawback and Trade-offs
//Just like any tool, there are drawback and trade-offs. Cascading values are no different.

//While it's early days I can see a couple of things which may end up becoming an issue when bigger, more real world applications become common.

//Over Use
//I can see cascading values being over used quite easily. I think you could see apps which end up declaring a load of cascading values in their main layouts, then every other component is declaring and using them as well.I think this could lead to code that's hard to understand and difficult to follow.  

//Time will tell with this and we won't really know till bigger applications get built so I guess we'll have to wait and see.

//Updating Values
//We looked at a couple of ways of updating a cascading value from a descendant earlier. In the event version, it was a simple example and the component which updated the value was declared within the same markup.

//But what if we wanted to trigger an update from a component deeper in the component tree that was declared in a different component? Let me show you an example.

//HTML<!-- Index.cshtml -->

//<CascadingValue Value = "@SomeValue" >
//    < ChildComponent ></ ChildComponent >
//< CascadingValue >

//@code {
//    string SomeValue { get; set;
//} = "Initial Value";
//}
//HTML < !--ChildComponent.cshtml-- >

//< AnotherChildComponent >< AnotherChildComponent >
//HTML < !--AnotherChildComponent.cshtml-- >

//< p > @SomeValue </ p >

//@code {
//  [CascadingParameter] string SomeValue { get; set; }

//  [Parameter] public Action<string> OnSomeValueChanged { get; set; }

//  private void ChangeValue()
//  {
//    OnSomeValueChanged?.Invoke("New Value");
//  }
//}

//With the setup we have above, how do we handle raising the OnSomeValueChanged event from the AnotherChildComponent to the Index component?

//The answer is we would probably have to declare an intermediate event on the ChildComponent as well. So the whole thing would look something like this.

//HTML<!-- Index.cshtml -->

//<CascadingValue Value="@SomeValue">
//    <ChildComponent OnChildSomeValueChanged="@UpdateValue"></ChildComponent>
//<CascadingValue>
    
//@code {
//    string SomeValue { get; set; } = "Initial Value";

//void UpdateValue(string newValue)
//{
//  SomeValue = newValue;
//  StateHasChanged();
//}
//}
//HTML < !--ChildComponent.cshtml-- >

//< AnotherChildComponent OnSomeValueChanged = "ChangeValue" >< AnotherChildComponent >

//@code {
//  [Parameter] public Action<string> OnChildSomeValueChanged { get; set; }

//  private void ChangeValue(string newValue)
//  {
//    OnSomeChildValueChanged?.Invoke(newValue);
//  }
//}

//HTML < !--AnotherChildComponent.cshtml-- >

//< p > @SomeValue </ p >

//@code {
//  [CascadingParameter] string SomeValue { get; set; }

//  [Parameter] public Action<string> OnSomeValueChanged { get; set; }

//  private void ChangeValue()
//  {
//    OnSomeValueChanged?.Invoke("New Value");
//  }
//}

//This is not good in my opinion and I would suggest that if you start going down this route, to consider using a common service to manage things.

//In fact, I think if you're passing object instances around as per the other updating example, then you should probably ask yourself if a service might be a better option as well.

//Summary
//That brings us to the end of this post. I hope you've managed to learn something here today. We've covered what cascading values and parameters are, some ways they can be used and some possible drawback to look out for.

//What are your opinions on them? Have you found any positives or negatives I've not mentioned. If so, please leave a comment below and tell me about your experience.