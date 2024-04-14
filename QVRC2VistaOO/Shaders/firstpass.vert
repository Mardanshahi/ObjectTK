
#version 330

in vec3 aPosition1;

uniform mat4 projection;
uniform mat4 view;
uniform mat4 model;

uniform vec3 origin;

out vec3 color;

void main()
{
    /* transform coordinates */
    gl_Position = projection * view * model * vec4(aPosition1, 1.0);
    /* pass position as color down the line */
    color = aPosition1;
}
