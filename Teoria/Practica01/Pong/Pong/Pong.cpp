#include <windows.h>
#include <GL/glut.h>
#include <math.h>
#include <string>


#define PI 3.1415926535898 

int width = 900, height = 500;
int interval = 16;

int scoreLeft = 0, scoreRight = 0;

int raqW = 20;
int raqH = 150;
int raqSp = 4;

float raqLeftX = 10.0f;
float raqLeftY = 240.0f;

float raqRightX = width - raqW - 10;
float raqRightY = 240.0f;

bool keyLeft[256] = { false };
bool keyRight[256] = { false };

double xpos = width / 2;
double ypos = height / 2;

float ydir = -1.0f;
float xdir = (rand() % 2 == 0) ? -1.0f : 1.0f;

int ballSize = 8;

double sx = 1.0, sy = 1.0, squash = 0.8;

float ballSp = 10.0f;
float speedIncr = 0.2f;
float maxSp = 25.0f;

GLint circle_points = 100;

void MyCircle2f(GLfloat centerx, GLfloat centery, GLfloat radius) {
    glBegin(GL_POLYGON);
    for (int i = 0; i < circle_points; i++) {
        double angle = 2 * PI * i / circle_points;
        glVertex2f(centerx + radius * cos(angle), centery + radius * sin(angle));
    }
    glEnd();
}

void reshape(int width, int height) {
    glViewport(0, 0, (GLsizei)width, (GLsizei)height);
    glMatrixMode(GL_PROJECTION);
    glLoadIdentity();
    gluOrtho2D(0.0, width, 0.0, height);
    glMatrixMode(GL_MODELVIEW);
    glLoadIdentity();
}

void displayText(float x, float y, std::string text) {
    glRasterPos2f(x, y);
    for (size_t i = 0; i < text.size(); ++i) {
        glutBitmapCharacter(GLUT_BITMAP_8_BY_13, text[i]);
    }
}

void displayRaq(float x, float y, float width, float height) {
    glBegin(GL_QUADS);
    glVertex2f(x, y);
    glVertex2f(x + width, y);
    glVertex2f(x + width, y + height);
    glVertex2f(x, y + height);
    glEnd();
}

void keyPressLeft(unsigned char key, int x, int y) { keyLeft[key] = true; }
void keyPressDropLeft(unsigned char key, int x, int y) { keyLeft[key] = false; }

void keyPressRight(int key, int x, int y) { keyRight[key] = true; }
void keyPressDropRight(int key, int x, int y) { keyRight[key] = false; }

void keyboard() {
    glutKeyboardFunc(keyPressLeft);
    glutKeyboardUpFunc(keyPressDropLeft);
    glutSpecialFunc(keyPressRight);
    glutSpecialUpFunc(keyPressDropRight);
}

void vec2_norm(float& x, float& y) {
    float length = sqrt((x * x) + (y * y));
    if (length != 0.0f) {
        length = 1.0f / length;
        x *= length;
        y *= length;
    }
}

void updateBall() {

    xpos += xdir * ballSp;
    ypos += ydir * ballSp;

    if (xpos < raqLeftX + raqW && xpos > raqLeftX && ypos < raqLeftY + raqH && ypos > raqLeftY) {

        float t = ((ypos - raqLeftY) / raqH) - 0.5f;
        xdir = fabs(xdir);

        ydir = t * 1.5f + ((rand() % 10 - 5) / 20.0f);

        ballSp = fmin(ballSp + speedIncr, maxSp);

        sx = squash;
        sy = 1.2;
    }

    if (xpos > raqRightX && xpos < raqRightX + raqW && ypos < raqRightY + raqH && ypos > raqRightY) {

        float t = ((ypos - raqRightY) / raqH) - 0.5f;
        xdir = -fabs(xdir);

        ydir = t * 1.5f + ((rand() % 10 - 5) / 20.0f);

        ballSp = fmin(ballSp + speedIncr, maxSp);

        sx = squash;
        sy = 1.2;
    }

    if (xpos < 0) {
        ++scoreRight;

        xpos = width / 2;
        ypos = height / 2;

        xdir = fabs(xdir);
        ydir = 0;

        sx = sy = 1.0;
        ballSp = 10.0f;
    }

    if (xpos > width) {
        ++scoreLeft;

        xpos = width / 2;
        ypos = height / 2;

        xdir = -fabs(xdir);
        ydir = 0;

        sx = sy = 1.0;
        ballSp = 10.0f;
    }

    if (ypos > height) {
        ydir = -fabs(ydir);

        sx = 1.2;
        sy = squash;
    }

    if (ypos < 0) {
        ydir = fabs(ydir);

        sx = 1.2;
        sy = squash;
    }

    vec2_norm(xdir, ydir);
}

void update(int value) {

    if (keyLeft['w'] && raqLeftY + raqH < height)
        raqLeftY += raqSp;

    if (keyLeft['s'] && raqLeftY > 0)
        raqLeftY -= raqSp;

    if (keyRight[GLUT_KEY_UP] && raqRightY + raqH < height)
        raqRightY += raqSp;

    if (keyRight[GLUT_KEY_DOWN] && raqRightY > 0)
        raqRightY -= raqSp;

    updateBall();

    glutTimerFunc(interval, update, 0);
    glutPostRedisplay();
}

void display() {

    glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
    glLoadIdentity();

    displayRaq(raqLeftX, raqLeftY, raqW, raqH);
    displayRaq(raqRightX, raqRightY, raqW, raqH);

    displayText(width / 2 - 10, height - 15,
        std::to_string(scoreLeft) + ":" + std::to_string(scoreRight));

    glPushMatrix();

    glTranslatef(xpos, ypos, 0);
    glScalef(sx, sy, 1.0);

    MyCircle2f(0, 0, ballSize);

    glPopMatrix();

    glutSwapBuffers();
}

int main(int argc, char* argv[]) {

    glutInit(&argc, argv);
    glutInitDisplayMode(GLUT_DOUBLE | GLUT_RGB | GLUT_DEPTH);

    glutInitWindowSize(width, height);
    glutCreateWindow("Pong - Atari");

    glutDisplayFunc(display);

    keyboard();

    glutTimerFunc(interval, update, 0);

    glutReshapeFunc(reshape);

    glColor3f(1.0f, 1.0f, 1.0f);

    glutMainLoop();

    return 1;
}