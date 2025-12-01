export interface StudentLeaveRequestDto {
  leaveId: string;
  studentId: string;
  fromDate: string;
  toDate: string;
  reason: string;
  status: string;
}

export interface CreateLeaveRequestDto {
  fromDate: string;
  toDate: string;
  reason: string;
}

export interface UpdateLeaveStatusDto {
  leaveId: string;
  status: string;
}
