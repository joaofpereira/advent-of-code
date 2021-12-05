import sys

curves = ['\\', '/']

class Cart:
    def __init__(self, inital_x, initial_y, direction):
        self.x = inital_x
        self.y = initial_y
        self.state = 0
        self.direction = direction

    def verify_state(self, current_char):
        if current_char == '+':
            if self.state == 0:
                if self.direction == 'L':
                    self.direction = 'D'
                elif self.direction == 'U':
                    self.direction = 'L'
                elif self.direction == 'R':
                    self.direction = 'U'
                else:
                    self.direction = 'D'
            elif self.state == 2:
                if self.direction == 'L':
                    self.direction = 'U'
                elif self.direction == 'U':
                    self.direction = 'R'
                elif self.direction == 'R':
                    self.direction = 'D'
                else:
                    self.direction = 'L'

            self.state = (self.state + 1) % 2

    def go_up(self):
        self.y = self.y + 1

    def go_down(self):
        self.y = self.y - 1

    def go_right(self):
        self.x = self.x + 1

    def go_left(self):
        self.x = self.x - 1

    def do_curve(self, direction, curve):
        if direction == 'R':
            self.go_right()
            if curve == '\\':
                self.direction = 'D'
            else:
                self.direction = 'U'
        elif direction == 'L':
            self.go_left()
            if curve == '\\':
                self.direction = 'U'
            else:
                self.direction = 'D'
        elif direction == 'U':
            self.go_up()
            if curve == '\\':
                self.direction = 'L'
            else:
                self.direction = 'R'
        else:
            self.go_down()
            if curve == '\\':
                self.direction = 'R'
            else:
                self.direction = 'L'

    def move(self, maze):
        self.verify_state(maze[self.y][self.x ])
        if self.direction == 'R':
            if maze[self.y][self.x + 1] in curves:
                self.do_curve(self.direction, maze[self.y][self.x + 1])
            else:
                self.go_right()
        elif self.direction == 'L':
            if maze[self.y][self.x - 1] in curves:
                self.do_curve(self.direction, maze[self.y][self.x - 1])
            else:
                self.go_left()
        elif self.direction == 'U':
            if maze[self.y + 1][self.x] in curves:
                self.do_curve(self.direction, maze[self.y + 1][self.x])
            else:
                self.go_up()
        else:
            if maze[self.y - 1][self.x] in curves:
                self.do_curve(self.direction, maze[self.y - 1][self.x])
            else:
                self.go_down()
        
    def __repr__(self):
        return 'X: ' + str(self.x) + ' Y: ' + str(self.y) + ' Direction: ' + str(self.direction) + ' State: ' + str(self.state)


def read_input():
    with open('./input.txt', 'r') as f:
        lines = f.readlines()
        return [[c for c in line.replace('\n','')] for line in lines]


def check_colision(carts):
    for i in range(len(carts)):
        for j in range(i + 1, len(carts)):
            if carts[i].x == carts[j].x and carts[i].y == carts[j].y:
                print (carts[i].x, carts[i].y)
                return True
    return False


def multisort(xs, specs):
    for key, reverse in reversed(specs):
        xs.sort(key=key, reverse=reverse)
    return xs

def get_cart_char(direction):
    if direction == 'R':
        return '>'
    elif direction == 'L':
        return '<'
    elif direction == 'U':
        return '^'
    else:
        return 'v'

def print_maze(maze, carts):
    for cart in carts:
        maze[cart.y][cart.x] = get_cart_char(cart.direction)
    for line in maze:
        print(''.join(line))
    print()

def main():    
    maze = read_input()
    carts = []
    for i in range(0, len(maze)):
        for j in range(0, len(maze[i])):
            if maze[i][j] == '<':
                carts.append(Cart(j, i, 'L'))
            elif maze[i][j] == '^':
                carts.append(Cart(j, i, 'U'))
            elif maze[i][j] == 'v':
                carts.append(Cart(j, i, 'D'))
            elif maze[i][j] == '>':
                carts.append(Cart(j, i, 'R'))

    print (carts)

    while True:
        print_maze(maze.copy(), carts)
        # carts = multisort(carts, (('y', True), ('x', False)))
        for cart in carts:
            if check_colision(carts):
                break
            cart.move(maze)

if __name__ == "__main__":
    main()