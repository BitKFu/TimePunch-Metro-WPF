# Tab Control
The Tab Control has been redesigned to match the metro style. That means that the control has no special chrome.

**Here's an example of the restyled TabControl**
[Image:TabControl.png)(Image_TabControl.png)

**How to define the TabControl in XAML**
Here's an example of how to implement the TabControl in XAML.
{{
<TabControl Style="{StaticResource WinTabControlStyle}">
    <TabControl.Items>
                
        <TabItem Header="TabItem 1" Style="{StaticResource WinTabItemStyle}">
            <TextBlock Text="This is the first tab item ..." Style="{StaticResource WinTextAccentStyle}"/>
        </TabItem>
                
        <TabItem Header="TabItem 2" Style="{StaticResource WinTabItemStyle}">
            <TextBlock Text="This is the second tab item ..." Style="{StaticResource WinTextAccentStyle}"/>
        </TabItem>
  
    </TabControl.Items>
</TabControl>
}}