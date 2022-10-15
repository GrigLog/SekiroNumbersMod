#pragma once
class V3 {
public:
    float x, y, z;
    V3(float x, float y, float z) {
        this->x = x;
        this->y = y;
        this->z = z;
    }

    V3() : V3(0, 0, 0) {}

    bool isZero() {
        return x == 0 && y == 0 && z == 0;
    }

    V3& normalize() {
        float len = length();
        x /= len;
        y /= len;
        z /= len;
        return *this;
    }

    V3 normalized() {
        return V3(x, y, z).normalize();
    }

    float length() const {
        return sqrtf(x * x + y * y + z * z);
    }

    static float distance(V3 a, V3 b) {
        return (a - b).length();
    }

    V3 cross(V3 b) const {
        return {y*b.z - z*b.y, z*b.x - x*b.z, x*b.y - y*b.x};
    }

    bool operator==(const V3 b) const {
        return x == b.x && y == b.y && z == b.z;
    }

    bool operator !=(V3 b) const {
        return !(*this == b);
    }

    V3 operator+(V3 b) const {
        return {x + b.x, y + b.y, z + b.z};
    }

    V3 operator-() const {
        return {-x, -y, -z};
    }

    V3 operator-(V3 b) const {
        return {x - b.x, y - b.y, z - b.z};
    }

    float operator*(V3 b) const {
        return x * b.x + y * b.y + z * b.z;
    }

    V3 operator*(float k) const {
        return {x * k, y * k, z * k};
    }

    V3 rotateY(float angle) const {
        return {x * cos(angle) + z * sin(angle), y, x * -sin(angle) + z * cos(angle)};
    }

    friend std::ostream& operator<< (std::ostream& out, const V3& v) {
        out << "{" << v.x << ", " << v.y << ", " << v.z << "}";
        return out;
    }
};