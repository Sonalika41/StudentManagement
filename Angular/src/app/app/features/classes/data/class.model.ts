export interface ClassDto {
  classId: string;
  name: string;
  section: string;
  classTeacherUserId: string;
  classTeacherEmail?: string;
  studentCount: number;
}

export interface CreateClassRequestDto {
  name: string;
  section: string;
  classTeacherUserId: string;
}

export interface UpdateClassRequestDto {
  name: string;
  section: string;
  classTeacherUserId: string;
}

export interface AssignClassTeacherDto {
  classId: string;
  teacherUserId: string;
}
