export class Pagination<T>{
    items: Array<T>;
    currentPage: number;
    itemsPerPage: number;
    totalCount: number;
    totalPages: number;
}

export class PaginationRequstDto{
    pageNumber: number;
    pageSize: number;
    orderBy: string = "";
    order: string = "";
    filter: string = "";
}