# Get up and running with Git

1. Version control system. 
2. Three main stages of a file:
    * **Commited:** file is save in local database (git commit).
    * **Modified:** when you change a commited file. Alter from last commited version. 
    * **Staged:** when you finish the changes the file moves to staged. He commits those changes and place them again in the commited phase. (git add, ready for commit). 
3. **Untracked:** any files added to the project since the last snapshot. To track them add it to the the project and Git puts it in staged state.
4. The three stages of a git project:
    * **Working directory:** a single copy. A check out of one version of the project. The uncommited changes remain in the working directory until you decide to commit them. If the files was modified since the last checkout of that file but have not being added to the staging area. 
    * **Staging area:** the index. Area that sits between the working directory  and the .git directory. Use it to build up a change or set of changes that it wants to commit by taking a snapshot of these changes. Files have being modified. 
    * **.git repository:** the origin of the project. What's pulled down from the remote server. It is also where Git stores the metadata and object database for his project. Files here are commited and recorded to the project as version snapshot. 

5. Check your git configuration:

    ```console
    git config --list
    ```
6. Initialize repository:
    * It added the git configuration to a .git subdirectory that has marked this folder and all of its content in version control. 
    ```console
    git init

    Initialized empty Git repository in D:/Desarrollo/Git/cafe_recipes/
    ```
7. To add new files to the staging area:
      ```console
    git add .

    ```  
8. Now that they're being added we can make a commit. 
    ```console
    git commit -m "First commit"

    [master (root-commit) 084f937] First commit
    1 file changed, 1 insertion(+)
    create mode 100644 README.md
    ```  

9. Connect local repository with cloud repository:

    ```console
    git remote add origin https://github.com/diego1389/wired-brain-recipes.git
    ```
10. To push our repository that now is pointing to a git hub repository:
    ```console
    git push -u origin master

    Fatal: HttpRequestException encountered.
    An error occurred while sending the request.
    Username for 'https://github.com': dalguillen3089@gmail.com
    Counting objects: 3, done.
    Writing objects: 100% (3/3), 256 bytes | 0 bytes/s, done.
    Total 3 (delta 0), reused 0 (delta 0)
    To https://github.com/diego1389/wired-brain-recipes.git
    * [new branch]      master -> master
    Branch master set up to track remote branch master from origin.
    ```
# Basic commands

1. You can check the status of your repositories at any time:
    ```console
    git status

    On branch master
    Your branch is up-to-date with 'origin/master'.
    nothing to commit, working tree clean


    --------------------
    On branch master
    Your branch is up-to-date with 'origin/master'.
    Untracked files:
    (use "git add <file>..." to include in what will be committed)

            vendors.txt

    nothing added to commit but untracked files present (use "git add" to track)
    ```
2. Add new file and stage it:
     ```console
      touch vendors.txt
      git add vendors.txt
      ```
3. Modify an existing file.
4. Add modified file to the stage area:
    ```console
    git add gingerbread_coffee
    git status

    On branch master
    Your branch is up-to-date with 'origin/master'.
    Changes to be committed:
    (use "git reset HEAD <file>..." to unstage)
            modified:   gingerbread_coffee
            new file:   vendors.txt
    ```
5. If you change a file after you stage it (git add) and you want those changes to be on the snapshot, you have to run git add again. 
6. A short status in a smaller way:
    ```console
    git status -s

    A  employees.txt
    M  gingerbread_coffee
    A  vendors.txt
    ```
    |Staged |Modified  | File name |
    |:-:|-|-|
    |M |  |File 1 | 
    |M  |M |File 2  |

    * **M:** modified
    * **A:** new files added to the staging area
    * **??:** untracked files
7. Git diff to see what changes have I staged that are ready to be commited and also what changes have I made but not staged.
8. To compare our staged changes to our last commited snapshot. 
    ```console
    git diff --staged
    ```
    * Compared files (same file but in different snapshot):
        - diff --git a/gingerbread_coffee b/gingerbread_coffee
    *  File metadata:
        - new file mode 100644
        - index 0000000..536c678     
    * Change markers for files A and B:
        - --- a/gingerbread_coffee
        - +++ b/gingerbread_coffee
    * Chunk header:
        - @@ -1 +1 @@
     ```console
    git diff --staged --no-renames
    ```
9. To compare files that have not being staged:

   ```console
    git diff
    
    diff --git a/vendors.txt b/vendors.txt
    index e69de29..81288b2 100644
    --- a/vendors.txt
    +++ b/vendors.txt
    @@ -0,0 +1 @@
    +Algo cambio 
    \ No newline at end of file
    ```
10. To skip the staging area and commit modified files:

    ```console
       git commit -a -m "Add new vendor"

        [master d95ed19] Add new vendor
        3 files changed, 3 insertions(+), 1 deletion(-)
        create mode 100644 employees.txt
        create mode 100644 vendors.txt
    ```
11. To push your commits to your remote repository:

    ```console
    git push origin master

    Counting objects: 5, done.
    Delta compression using up to 8 threads.
    Compressing objects: 100% (3/3), done.
    Writing objects: 100% (5/5), 468 bytes | 0 bytes/s, done.
    Total 5 (delta 2), reused 0 (delta 0)
    remote: Resolving deltas: 100% (2/2), completed with 2 local objects.
    To https://github.com/diego1389/wired-brain-recipes.git
    338b2a6..d95ed19  master -> master
    ```
12. To check the commits history in reverse chronological manner:

    ```console
    git log

    commit d95ed19a9d6b2f834ccfae8ebac4561dff1241d1
    Author: Diego Guillén Hernández <dalguillen3089@gmail.com>
    Date:   Sat Aug 22 11:15:44 2020 -0600

    Add new vendor

    commit 338b2a6f62890a21d866eaa13097fdefa53b3cc5
    Author: Diego Guillén Hernández <dalguillen3089@gmail.com>
    Date:   Tue Aug 18 18:52:13 2020 -0600

    Add recipes

    commit 084f937686150c121b259f8051c72ccbb689b59c
    Author: Diego Guillén Hernández <dalguillen3089@gmail.com>
    Date:   Tue Aug 18 18:44:33 2020 -0600

    First commit 
    ```
13. To limit the number of commits to display in the history:

    ```console
    git log -2
    ```
14. To display the commits in a single line format:
    ```console
    git log --oneline
    ```
15. To get a detailed view of all of our commits:
    ```console
    git log --stat

    commit d95ed19a9d6b2f834ccfae8ebac4561dff1241d1
    Author: Diego Guillén Hernández <dalguillen3089@gmail.com>
    Date:   Sat Aug 22 11:15:44 2020 -0600

        Add new vendor

    employees.txt      | 1 +
    gingerbread_coffee | 2 +-
    vendors.txt        | 1 +
    3 files changed, 3 insertions(+), 1 deletion(-)

    commit 338b2a6f62890a21d866eaa13097fdefa53b3cc5
    Author: Diego Guillén Hernández <dalguillen3089@gmail.com>
    Date:   Tue Aug 18 18:52:13 2020 -0600

        Add recipes

    cappuccino_cooler           | 1 +
    coconut_oil_coffee          | 1 +
    eggnog_latte                | 1 +
    gingerbread_coffee          | 1 +

    ```
16. To get all the information of the commit (similar to git diff info):

    ```console
    git log --patch -1

    commit d95ed19a9d6b2f834ccfae8ebac4561dff1241d1
    Author: Diego Guillén Hernández <dalguillen3089@gmail.com>
    Date:   Sat Aug 22 11:15:44 2020 -0600

    Add new vendor

    diff --git a/employees.txt b/employees.txt
    new file mode 100644
    index 0000000..536c678
    --- /dev/null
    +++ b/employees.txt
    @@ -0,0 +1 @@
    +Josh Russel | 555-555
    \ No newline at end of file
    diff --git a/gingerbread_coffee b/gingerbread_coffee
    index 75438e3..de5eb74 100644
    --- a/gingerbread_coffee
    +++ b/gingerbread_coffee
    @@ -1 +1 @@
    -1/2 cup molasses 1/4 cup brown sugar 1/2 teaspoon baking soda 1 teaspoon ground ginger 3/4 teaspoon ground cinnamon 6 cups hot brewed coffee 1 cup half-and-half cream 1 teaspoon ground cloves 1 1/2 cups sweetened whipped cream
    +1/2 cup molasses 1/4 cup white sugar 1/2 teaspoon baking soda 1 teaspoon ground ginger 3/4 teaspoon ground cinnamon 6 cups hot brewed coffee 1 cup half-and-half cream 1 teaspoon ground cloves 1 1/2 cups sweetened whipped cream
    diff --git a/vendors.txt b/vendors.txt
    new file mode 100644
    index 0000000..76e97f0
    --- /dev/null
    +++ b/vendors.txt
    @@ -0,0 +1 @@
    +John Coffee Cups
    ```

17. Read document about commit messages:https://chris.beams.io/posts/git-commit/

    1. If needed separate subject from body.
    2. Limit the subject line to 50 characters.
    3. Capitalize the subject line.
    4. Do not end the subject line with a period.
    5. Use the impertive mood in the subject line.
    6. Wrap the body at 72 characters if needed.
    7. Use the body to explain what a why, not how.  

18. To make git stop tracking a file. The next time you commit the file will no longer be tracked by git and will be removed from the project. 

```console
git rm eggnog_latte

rm 'eggnog_latte'

git status
On branch master
Your branch is up-to-date with 'origin/master'.
Changes to be committed:
  (use "git reset HEAD <file>..." to unstage)

        deleted:    eggnog_latte
```

19. If you want to remove the file from git but keep the local file:

```console
git rm --cached .\irish_coffee
rm 'irish_coffee'

git status
On branch master
Your branch is up-to-date with 'origin/master'.
Changes to be committed:
  (use "git reset HEAD <file>..." to unstage)

        deleted:    eggnog_latte
        deleted:    irish_coffee

Untracked files:
  (use "git add <file>..." to include in what will be committed)

        irish_coffee
```

20. To rename a file:

```console
mv .\README.md README
```

21. To create a new branch (checkout to switch branch, -b to create new one):
```console
git checkout -b new_branch

D       README.md
D       eggnog_latte
D       irish_coffee
Switched to a new branch 'new_branch'

git status
On branch new_branch
Changes to be committed:
  (use "git reset HEAD <file>..." to unstage)

        deleted:    eggnog_latte
        deleted:    irish_coffee

Changes not staged for commit:
  (use "git add/rm <file>..." to update what will be committed)
  (use "git checkout -- <file>..." to discard changes in working directory)

        deleted:    README.md

Untracked files:
  (use "git add <file>..." to include in what will be committed)

        README
        irish_coffee
```

22. When you checkout a different branch from master, changes on the staging area appear in the new branch as well.

23. Commit changes on the new branch:

```console
git commit -m "Rename README and remove eggnog_latte"

[new_branch b7244bc] Rename README and remove eggnog_latte
 2 files changed, 2 deletions(-)
 delete mode 100644 eggnog_latte
 delete mode 100644 irish_coffee
 ```

24. If we try to checkout to our master branch again we get an error:
    ```console
    error: The following untracked working tree files would be overwritten by checkout:
            irish_coffee
    Please move or remove them before you switch branches.
    Aborting
    ```
    - Since we untracked our file with git, we deleted the version control cached but did not remove or deleted from our file system. We can remove it or we can add it back. But lets say we dont want to get rid of it nor added back yet.

    ```console
    git add .

    warning: LF will be replaced by CRLF in README.
    The file will have its original line endings in your working directory.
    warning: LF will be replaced by CRLF in irish_coffee.
    The file will have its original line endings in your working directory.
   
    git stash
   
    Saved working directory and index state WIP on new_branch: b7244bc Rename README and remove eggnog_latte
    HEAD is now at b7244bc Rename README and remove eggnog_latte
    
    ```

    - It saves our work in a Work In Progress state in our branch.

    - We can see that after stashing our branch is clean and nothing to commit and our working directory is clean as well.

      ```console
        git status

        On branch new_branch
        nothing to commit, working tree clean
      ```
25. To check our Work In Progress changes: 
    ```console
     git stash list
    
    stash@{0}: WIP on new_branch: b7244bc Rename README and remove eggnog_latte
     ```
26. To see a better detailed view:
    ```console
    git stash show

    README.md => README | 0
    irish_coffee        | 1 +
    2 files changed, 1 insertion(+)
    ```

27. To get a change out of stash (get it back to the working directory):
    ```console
    git stash pop
    ```
28. If you checkout to master the eggnog_late is still there and the README.md file hasnt change its name.

29. Add new file in master and commit.
```console
git status

On branch master
Your branch is ahead of 'origin/master' by 1 commit.
  (use "git push" to publish your local c
```
30. To merge the new branch into master:

```console
git merge new_branch

Removing irish_coffee
Removing eggnog_latte
Merge made by the 'recursive' strategy.
 eggnog_latte | 1 -
 irish_coffee | 1 -
 2 files changed, 2 deletions(-)
 delete mode 100644 eggnog_latte
```
31. Once we merge some commits with the master, we still can reset some. Reset command let us move commits from history to our working directory or staging area. Not recommended when you have already pushed changes to origin.

32. To move the commits to the staging area
```console
git reset --soft
```
33. To move the commits to the working directory 
```console
git reset d95ed19a9d6b2f834ccfae8ebac4561dff1241d1

Unstaged changes after reset:
D       eggnog_latte
D       irish_coffee

git status
On branch master
Your branch is up-to-date with 'origin/master'.
Changes not staged for commit:
  (use "git add/rm <file>..." to update what will be committed)
  (use "git checkout -- <file>..." to discard changes in working directory)

        deleted:    eggnog_latte
        deleted:    irish_coffee

Untracked files:
  (use "git add <file>..." to include in what will be committed)

        cold_brew.txt.txt

git push origin master
```
34. To start anew
```console
git reset --hard
```
35.  To clone the repository:

```console
git clone https://github.com/diego1389/wired-brain-recipes.git
```