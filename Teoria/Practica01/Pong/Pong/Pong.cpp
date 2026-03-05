#include <windows.h>
#include <GL/glut.h>
#include <math.h>

#define PI 3.1415926535898 

int width = 900, height = 500;
int interval = 16;

double xpos = width / 2;
double ypos = height / 2;

float xdir = 1.0f;
float ydir = -1.0f;

float ballSp = 8.0f;
int ballSize = 8;

GLint circle_points = 100;

void MyCircle2f(GLfloat centerx, GLfloat centery, GLfloat radius) {
    GLint i;
    GLdouble angle;
    glBegin(GL_POLYGON);
    for (i = 0; i < circle_points; i++) {
        angle = 2 * PI * i / circle_points;
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

void updateBall() {
    xpos += xdir * ballSp;
    ypos += ydir * ballSp;

    if (ypos > height || ypos < 0) {
        ydir *= -1;
    }

    if (xpos > width || xpos < 0) {
        xdir *= -1;
    }
}

void update(int value) {
    updateBall();
    glutTimerFunc(interval, update, 0);
    glutPostRedisplay();
}

void display(void) {
    glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
    glLoadIdentity();

    glPushMatrix();
    glTranslatef(xpos, ypos, 0);
    MyCircle2f(0, 0, ballSize);
    glPopMatrix();

    glutSwapBuffers();
}

int main(int argc, char* argv[]) {

    glutInit(&argc, argv);
    glutInitDisplayMode(GLUT_DOUBLE | GLUT_RGB | GLUT_DEPTH);
    glutInitWindowSize(width, height);
    glutCreateWindow("Pong");

    glutDisplayFunc(display);
    glutTimerFunc(interval, update, 0);
    glutReshapeFunc(reshape);

    glColor3f(1.0f, 1.0f, 1.0f);
    glutMainLoop();

    return 1;
}