﻿import {Applicant} from "./Applicant";
import {Filters} from "./Filters";

export interface UserList {
  items: UserAdminView[],
  totalItems: number,
  pageIndex: number,
  pageSize: number
}

export interface UserAdminView extends Applicant{

}

export interface UserFilters{
  searchTerm: string,
  pageIndex: number,
  pageSize: number
}

export const UserFiltersDefault: UserFilters = {
  searchTerm: "",
  pageSize: 10,
  pageIndex: 0,
}
