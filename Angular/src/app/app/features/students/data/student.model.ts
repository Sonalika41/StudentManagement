export interface StudentDto {
  studentID: string;
  fullName: string;
  age: number;
  email: string;
  course: string;
  enrollmentDate: string;
  marks: number;
  attendancePercentage: number;
  guardianContact?: string;
  classId?: string;
  className?: string;
  classSection?: string;
  classTeacherEmail?: string;
}

export interface CreateStudentRequestDto {
  fullName: string;
  age: number;
  email: string;
  course: string;
  enrollmentDate: string;
  marks: number;
  attendancePercentage: number;
  guardianContact?: string;
  classId?: string;
}

export interface UpdateStudentRequestDto extends CreateStudentRequestDto {}
export interface TeacherUpdateStudentDto { email: string; marks: number; }
