export interface Filters {
  pageSize: number,
  pageIndex: number,
  searchString: string,
  category: string,
  levels: any,
  //TODO: More filters and sort order

}

export function newFilters(): Filters {
  return {
    pageSize: 2,
    pageIndex: 1,
    searchString: "",
    category: "",
    levels: []
  }
}
