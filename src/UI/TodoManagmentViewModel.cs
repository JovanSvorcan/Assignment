using System.Windows.Input;
using Assignment.Application.Cache;
using Assignment.Application.Common.Models;
using Assignment.Application.TodoItems.Commands.DoneTodoItem;
using Assignment.Application.TodoItems.Queries.GetItems;
using Assignment.Application.TodoLists.Queries.GetTodos;
using Caliburn.Micro;
using MediatR;

namespace Assignment.UI;
internal class TodoManagmentViewModel : Screen
{
    private readonly ISender _sender;
    private readonly IWindowManager _windowManager;

    private IList<LookupDto> todoLists;
    public IList<LookupDto> TodoLists
    {
        get
        {
            return todoLists;
        }
        set
        {
            todoLists = value;
            NotifyOfPropertyChange(() => TodoLists);
        }
    }

    private LookupDto _selectedTodoList;
    public LookupDto SelectedTodoList
    {
        get => _selectedTodoList;
        set
        {
            _selectedTodoList = value;
            NotifyOfPropertyChange(() => SelectedTodoList);

            if (SelectedTodoList != null)
            {
                Task.Run(GetListItems);
            }
        }
    }

    private IList<TodoItemDto> todoItems;
    public IList<TodoItemDto> TodoItems
    {
        get
        {
            return todoItems;
        }
        set
        {
            todoItems = value;
            NotifyOfPropertyChange(() => TodoItems);
        }
    }

    private TodoItemDto _selectedItem;
    public TodoItemDto SelectedItem
    {
        get => _selectedItem;
        set
        {
            _selectedItem = value;
            NotifyOfPropertyChange(() => SelectedItem);

            IsDoneEnabled = SelectedItem == null ? true : !SelectedItem.Done;
            NotifyOfPropertyChange(() => IsDoneEnabled);
        }
    }

    public ICommand AddTodoListCommand { get; private set; }
    public ICommand AddTodoItemCommand { get; private set; }
    public ICommand DoneTodoItemCommand { get; private set; }

    public bool IsDoneEnabled { get; set; }

    public TodoManagmentViewModel(ISender sender, IWindowManager windowManager)
    {
        _sender = sender;
        _windowManager = windowManager;
        Initialize();
    }

    private async void Initialize()
    {
        await RefereshTodoLists();

        AddTodoListCommand = new RelayCommand(AddTodoList);
        AddTodoItemCommand = new RelayCommand(AddTodoItem);
        DoneTodoItemCommand = new RelayCommand(DoneTodoItem);
    }

    private async Task RefereshTodoLists()
    {
        var selectedListId = SelectedTodoList?.Id;

        TodoLists = await _sender.Send(new GetTodosQuery());

        if (selectedListId.HasValue && selectedListId.Value > 0)
        {
            SelectedTodoList = TodoLists.FirstOrDefault(list => list.Id == selectedListId.Value);
        }
    }

    private async Task GetListItems()
    {
        if (InMemoryCache<int, IList<TodoItemDto>>.TryGet(SelectedTodoList.Id, out var list))
        {
            TodoItems = list;
        }
        else
        {
            TodoItems = await _sender.Send(new GetItemsQuery(SelectedTodoList.Id));
            InMemoryCache<int, IList<TodoItemDto>>.AddOrUpdate(SelectedTodoList.Id, TodoItems);
        }
    }

    private async void AddTodoList(object obj)
    {
        var todoList = new TodoListViewModel(_sender);
        await _windowManager.ShowDialogAsync(todoList);
        await RefereshTodoLists();
    }

    private async void AddTodoItem(object obj)
    {
        var todoItem = new TodoItemViewModel(_sender, SelectedTodoList.Id);
        await _windowManager.ShowDialogAsync(todoItem);
        await RefereshTodoLists();
    }

    private async void DoneTodoItem(object obj)
    {
        await _sender.Send(new DoneTodoItemCommand(SelectedItem.Id));
        await RefereshTodoLists();
    }
}
