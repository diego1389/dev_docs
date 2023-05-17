- Helps you build UI components isolated from the business logic and context of your app. 

# Clone the template
npx degit chromaui/intro-storybook-vue-template taskbox

cd taskbox

# Install dependencies
yarn

Now we can quickly check that the various environments of our application are working properly:

# Start the component explorer on port 6006:
yarn storybook

# Run the frontend app proper on port 5173:
yarn dev

- Create a new component in the project:
- Task.vue
```js
<template>
    <div class="list-item">
      <label for="title" :aria-label="task.title">
        <input type="text" readonly :value="task.title" id="title" name="title" />
      </label>
    </div>
  </template>
  <script>
  export default {
    // eslint-disable-next-line vue/multi-word-component-names
    name: 'Task',
    props: {
      task: {
        type: Object,
        required: true,
        default: () => ({ id: '', state: '', title: '' }),
        validator: (task) => ['id', 'state', 'title'].every((key) => key in task)
      }
    }
  }
  </script>
```
- Create a new story file
- Task.stories.js
```js
import Task from './Task.vue';

import { action } from '@storybook/addon-actions';

export default {
  component: Task, //the component itself
  title: 'Task', // how to group or categorize the component in the Storybook sidebar
  tags: ['autodocs'], //to automatically generate documentation for our components
  //👇 Our events will be mapped in Storybook UI
  argTypes: { // specify the args behavior in each story
    onPinTask: {},
    onArchiveTask: {},
  },
  //👇 Our exports that end in "Data" are not stories.
  excludeStories: /.*Data$/, // additional information required by the story but should not be rendered in Storybook
};

export const actionsData = {
  onPinTask: action('pin-task'),
  onArchiveTask: action('archive-task'),
};

export const Default = {
  args: {
    task: {
      id: '1',
      title: 'Test Task',
      state: 'TASK_INBOX',
    },
  },
};

export const Pinned = {
  args: {
    task: {
      ...Default.args.task,
      state: 'TASK_PINNED',
    },
  },
};

export const Archived = {
  args: {
    task: {
      ...Default.args.task,
      state: 'TASK_ARCHIVED',
    },
  },
};
```
- Two levels of organization in Storybook: component and story. Think of each story as a permutation of a component. Action() to stub functions and state. 
- Modify the component to handle states and change styling for each state (story):
- Task.vue
```js
<template>
  <div :class="classes">
    <label
      :for="'checked' + task.id"
      :aria-label="'archiveTask-' + task.id"
      class="checkbox"
    >
      <input
        type="checkbox"
        :checked="isChecked"
        disabled
        :name="'checked' + task.id"
        :id="'archiveTask-' + task.id"
      />
      <span class="checkbox-custom" @click="archiveTask" />
    </label>
    <label :for="'title-' + task.id" :aria-label="task.title" class="title">
      <input
        type="text"
        readonly
        :value="task.title"
        :id="'title-' + task.id"
        name="title"
        placeholder="Input title"
      />
    </label>
    <button
      v-if="!isChecked"
      class="pin-button"
      @click="pinTask"
      :id="'pinTask-' + task.id"
      :aria-label="'pinTask-' + task.id"
    >
      <span class="icon-star" />
    </button>
  </div>
</template>

<script>
import { reactive, computed } from 'vue';

export default {
  // eslint-disable-next-line vue/multi-word-component-names
  name: 'Task',
  props: {
    task: {
      type: Object,
      required: true,
      default: () => ({ id: '', state: '', title: '' }),
      validator: task => ['id', 'state', 'title'].every(key => key in task),
    },
  },
  emits: ['archive-task', 'pin-task'],

  setup(props, { emit }) {
    props = reactive(props);
    return {
      classes: computed(() => ({
        'list-item TASK_INBOX': props.task.state === 'TASK_INBOX',
        'list-item TASK_PINNED': props.task.state === 'TASK_PINNED',
        'list-item TASK_ARCHIVED': props.task.state === 'TASK_ARCHIVED',
      })),
      /**
       * Computed property for checking the state of the task
       */
      isChecked: computed(() => props.task.state === 'TASK_ARCHIVED'),
      /**
       * Event handler for archiving tasks
       */
      archiveTask() {
        emit('archive-task', props.task.id);
      },
      /**
       * Event handler for pinning tasks
       */
      pinTask() {
        emit('pin-task', props.task.id);
      },
    };
  },
};
</script>
```
## Composite component:##
- Decorators:  Decorators are a way to provide arbitrary wrappers to stories. In this case we’re using a decorator key on the default export to add some margin around the rendered component. But they can also be used to add other context to components. 
- Add new TaskList.vue component
```js

<template>
    <div class="list-items">
      <template v-if="loading">
       <div v-for="n in 6" :key="n" class="loading-item">
         <span class="glow-checkbox" />
         <span class="glow-text">
           <span>Loading</span> <span>cool</span> <span>state</span>
         </span>
       </div>
      </template>
  
      <div v-else-if="isEmpty" class="list-items">
       <div class="wrapper-message">
         <span class="icon-check" />
         <p class="title-message">You have no tasks</p>
         <p class="subtitle-message">Sit back and relax</p>
       </div>
      </div>
  
      <template v-else>
       <Task
         v-for="task in tasksInOrder"
         :key="task.id"
         :task="task"
         @archive-task="onArchiveTask"
         @pin-task="onPinTask"
       />
     </template>
    </div>
  </template>
  
  <script>
  import Task from './Task.vue';
  import { reactive, computed } from 'vue';
  
  export default {
    name: 'TaskList',
    components: { Task },
    props: {
      tasks: { type: Array, required: true, default: () => [] },
      loading: { type: Boolean, default: false },
    },
    emits: ['archive-task', 'pin-task'],
  
    setup(props, { emit }) {
      props = reactive(props);
      return {
        isEmpty: computed(() => props.tasks.length === 0),
       tasksInOrder:computed(()=>{
         return [
           ...props.tasks.filter(t => t.state === 'TASK_PINNED'),
           ...props.tasks.filter(t => t.state !== 'TASK_PINNED'),
         ]
       }),
        /**
         * Event handler for archiving tasks
         */
        onArchiveTask(taskId) {
          emit('archive-task',taskId);
        },
        /**
         * Event handler for pinning tasks
         */
        onPinTask(taskId) {
          emit('pin-task', taskId);
        },
      };
    },
  };
  </script>
```
- Now create a TaskList's test states in the story file (TaskList.stories.js)
```js

import TaskList from './TaskList.vue';

import * as TaskStories from './Task.stories';

export default {
  component: TaskList,
  title: 'TaskList',
  tags: ['autodocs'],
  decorators: [() => ({ template: '<div style="margin: 3em;"><story/></div>' })],
  argTypes: {
    onPinTask: {},
    onArchiveTask: {},
  },
};

export const Default = {
  args: {
    // Shaping the stories through args composition.
    // The data was inherited from the Default story in task.stories.js.
    tasks: [
      { ...TaskStories.Default.args.task, id: '1', title: 'Task 1' },
      { ...TaskStories.Default.args.task, id: '2', title: 'Task 2' },
      { ...TaskStories.Default.args.task, id: '3', title: 'Task 3' },
      { ...TaskStories.Default.args.task, id: '4', title: 'Task 4' },
      { ...TaskStories.Default.args.task, id: '5', title: 'Task 5' },
      { ...TaskStories.Default.args.task, id: '6', title: 'Task 6' },
    ],
  },
};

export const WithPinnedTasks = {
  args: {
    // Shaping the stories through args composition.
    // Inherited data coming from the Default story.
    tasks: [
      ...Default.args.tasks.slice(0, 5),
      { id: '6', title: 'Task 6 (pinned)', state: 'TASK_PINNED' },
    ],
  },
};

export const Loading = {
  args: {
    tasks: [],
    loading: true,
  },
};

export const Empty = {
  args: {
    // Shaping the stories through args composition.
    // Inherited data coming from the Loading story.
    ...Loading.args,
    loading: false,
  },
};
```