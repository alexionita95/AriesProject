#version 330

struct Material {
    sampler2D diffuse;
    sampler2D specular;

    float shininess; 
};

struct Light {
    vec3 position;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

uniform Light light;
uniform Material material;
//We still need the view position.
uniform vec3 viewPos;

out vec4 FragColor;

in vec3 Normal;
in vec3 FragPos;
in vec2 TexCoords;

void main()
{
    //ambient
    vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords));

    //diffuse 
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(light.position - FragPos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, TexCoords));

    //specular
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular = light.specular * spec * vec3(texture(material.specular, TexCoords));

    //Now the result sum has changed a bit, since we now set the objects color in each element, we now dont have to
    //multiply the light with the object here, instead we do it for each element seperatly. This allows much better control
    //over how each element is applied to different objects.
    vec3 result = ambient + diffuse + specular;
    FragColor = vec4(result, 1.0);
}