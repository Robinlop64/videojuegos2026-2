#include <windows.h>
#include <GL/glut.h>
#include <math.h>

#define PI 3.1415926535898

int width = 900, height = 500;
int interval = 16;

// Raquetas
int raqW = 20;
int raqH = 150;
int raqSp = 6;

float raqLeftX = 10.0f;
float raqLeftY = 240.0f;

float raqRightX = 0.0f; 
float raqRightY = 240.0f;

// Teclas
bool keyLeft[256] = { false };
bool keyRight[256] = { false };

// Bola
double xpos = width / 2;
double ypos = height / 2;
float xdir = 1.0f;
float ydir = -0.6f;
float ballSp = 8.0f;
int ballSize = 8;

GLint circle_points = 100;

void MyCircle2f(GLfloat centerx, GLfloat centery, GLfloat radius) {
    glBegin(GL_POLYGON);
    for (int i = 0; i < circle_points; i++) {
        double angle = 2 * PI * i / circle_points;
        glVertex2f(centerx + radius * cos(angle), centery + radius * sin(angle));
    }
    glEnd();
}

void displayRaq(float x, float y, float w, float h) {
    glBegin(GL_QUADS);
    glVertex2f(x, y);
    glVertex2f(x + w, y);
    glVertex2f(x + w, y + h);
    glVertex2f(x, y + h);
    glEnd();
}

void reshape(int w, int h) {
    glViewport(0, 0, (GLsizei)w, (GLsizei)h);
    glMatrixMode(GL_PROJECTION);
    glLoadIdentity();
    gluOrtho2D(0.0, w, 0.0, h);
    glMatrixMode(GL_MODELVIEW);
    glLoadIdentity();

    width = w;
    height = h;
    raqRightX = width - raqW - 10;
}

void vec2_norm(float& x, float& y) {
    float length = sqrt((x * x) + (y * y));
    if (length != 0.0f) {
        length = 1.0f / length;
        x *= length;
        y *= length;
    }
}

// Input: izquierda 
void keyDown(unsigned char key, int, int) { keyLeft[key] = true; }
void keyUp(unsigned char key, int, int) { keyLeft[key] = false; }

// Input: derecha 
void specialDown(int key, int, int) { keyRight[key] = true; }
void specialUp(int key, int, int) { keyRight[key] = false; }

void keyboard() {
    glutKeyboardFunc(keyDown);
    glutKeyboardUpFunc(keyUp);
    glutSpecialFunc(specialDown);
    glutSpecialUpFunc(specialUp);
}

void resetBall(int direction) {
    xpos = width / 2;
    ypos = height / 2;

    xdir = (direction >= 0) ? 1.0f : -1.0f;
    ydir = (rand() % 2 == 0) ? 0.6f : -0.6f;

    vec2_norm(xdir, ydir);
}

void updateBall() {
    xpos += xdir * ballSp;
    ypos += ydir * ballSp;

    // Rebote paredes superior/inferior
    if (ypos > height) {
        ypos = height;
        ydir = -fabs(ydir);
    }
    if (ypos < 0) {
        ypos = 0;
        ydir = fabs(ydir);
    }

    // Colisión raqueta izquierda 
    if (xpos - ballSize < raqLeftX + raqW &&
        xpos + ballSize > raqLeftX &&
        ypos + ballSize > raqLeftY &&
        ypos - ballSize < raqLeftY + raqH) {

        float t = ((ypos - raqLeftY) / raqH) - 0.5f;
        xdir = fabs(xdir);
        ydir = t * 1.5f;
        vec2_norm(xdir, ydir);

        xpos = raqLeftX + raqW + ballSize; 
    }

    // Colisión raqueta derecha
    if (xpos + ballSize > raqRightX &&
        xpos - ballSize < raqRightX + raqW &&
        ypos + ballSize > raqRightY &&
        ypos - ballSize < raqRightY + raqH) {

        float t = ((ypos - raqRightY) / raqH) - 0.5f;
        xdir = -fabs(xdir);
        ydir = t * 1.5f;
        vec2_norm(xdir, ydir);

        xpos = raqRightX - ballSize;
    }


    if (xpos < 0) resetBall(+1);
    if (xpos > width) resetBall(-1);
}

void update(int) {
    // Raqueta izquierda
    if (keyLeft['w'] && raqLeftY + raqH < height) raqLeftY += raqSp;
    if (keyLeft['s'] && raqLeftY > 0) raqLeftY -= raqSp;

    // Raqueta derecha
    if (keyRight[GLUT_KEY_UP] && raqRightY + raqH < height) raqRightY += raqSp;
    if (keyRight[GLUT_KEY_DOWN] && raqRightY > 0) raqRightY -= raqSp;

    updateBall();

    glutTimerFunc(interval, update, 0);
    glutPostRedisplay();
}

void display() {
    glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
    glLoadIdentity();

    // Raquetas
    displayRaq(raqLeftX, raqLeftY, raqW, raqH);
    displayRaq(raqRightX, raqRightY, raqW, raqH);

    // Bola
    glPushMatrix();
    glTranslatef((float)xpos, (float)ypos, 0.0f);
    MyCircle2f(0, 0, (float)ballSize);
    glPopMatrix();

    glutSwapBuffers();
}

int main(int argc, char* argv[]) {
    glutInit(&argc, argv);
    glutInitDisplayMode(GLUT_DOUBLE | GLUT_RGB | GLUT_DEPTH);
    glutInitWindowSize(width, height);
    glutCreateWindow("Pong - Version 2");

    raqRightX = width - raqW - 10;

    glutDisplayFunc(display);
    glutReshapeFunc(reshape);
    keyboard();

    glutTimerFunc(interval, update, 0);

    glColor3f(1.0f, 1.0f, 1.0f);
    resetBall((rand() % 2 == 0) ? 1 : -1);

    glutMainLoop();
    return 0;
}